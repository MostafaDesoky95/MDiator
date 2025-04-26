using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MDiator
{
    public static class EventInvokerCache
    {
        private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, object, CancellationToken, Task>> _cache = new();

        public static Task Invoke(IServiceProvider provider, object @event, CancellationToken cancellationToken)
        {
            var eventType = @event.GetType();
            var invoker = _cache.GetOrAdd(eventType, BuildInvoker);
            return invoker(provider, @event, cancellationToken);
        }

        private static Func<IServiceProvider, object, CancellationToken, Task> BuildInvoker(Type eventType)
        {
            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var eventParam = Expression.Parameter(typeof(object), "event");
            var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

            var handlerInterfaceType = typeof(IMDiatorEventHandler<>).MakeGenericType(eventType);
            var handleMethod = handlerInterfaceType.GetMethod("Handle")!;

            var handlersEnumerable = Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetServices),
                new[] { handlerInterfaceType },
                spParam
            );

            var handlersVar = Expression.Variable(typeof(IEnumerable<>).MakeGenericType(handlerInterfaceType), "handlers");
            var handlersArrayVar = Expression.Variable(handlerInterfaceType.MakeArrayType(), "handlersArray");
            var resultTaskVar = Expression.Variable(typeof(Task), "resultTask");
            var tasksVar = Expression.Variable(typeof(List<Task>), "tasks");
            var handlerVar = Expression.Variable(handlerInterfaceType, "handler");

            var assignHandlers = Expression.Assign(handlersVar, handlersEnumerable);

            var assignHandlersArray = Expression.Assign(
                handlersArrayVar,
                Expression.Call(typeof(Enumerable), nameof(Enumerable.ToArray), new[] { handlerInterfaceType }, handlersVar)
            );

            var handlersLength = Expression.PropertyOrField(handlersArrayVar, "Length");
            var zero = Expression.Constant(0);
            var one = Expression.Constant(1);

            var callSingleHandle = Expression.Call(
                Expression.ArrayIndex(handlersArrayVar, zero),
                handleMethod,
                Expression.Convert(eventParam, eventType),
                cancellationTokenParam
            );

            var callHandle = Expression.Call(
                handlerVar,
                handleMethod,
                Expression.Convert(eventParam, eventType),
                cancellationTokenParam
            );

            var addTask = Expression.Call(
                tasksVar,
                typeof(List<Task>).GetMethod(nameof(List<Task>.Add))!,
                callHandle
            );

            var foreachHandlers = handlersArrayVar.ForEach(handlerVar, addTask);

            var whenAllCall = Expression.Call(
                typeof(Task),
                nameof(Task.WhenAll),
                Type.EmptyTypes,
                tasksVar
            );

            var assignCompletedTask = Expression.Assign(resultTaskVar, Expression.Constant(Task.CompletedTask, typeof(Task)));
            var assignSingleHandle = Expression.Assign(resultTaskVar, callSingleHandle);
            var assignWhenAll = Expression.Assign(resultTaskVar, whenAllCall);

            var ifZeroHandlers = Expression.IfThen(
                Expression.Equal(handlersLength, zero),
                assignCompletedTask
            );

            var ifOneHandler = Expression.IfThen(
                Expression.Equal(handlersLength, one),
                assignSingleHandle
            );

            var elseManyHandlers = Expression.Block(
                Expression.Assign(tasksVar, Expression.New(typeof(List<Task>))),
                foreachHandlers,
                assignWhenAll
            );

            var block = Expression.Block(
                new[] { handlersVar, handlersArrayVar, tasksVar, resultTaskVar },
                assignHandlers,
                assignHandlersArray,
                ifZeroHandlers,
                Expression.IfThenElse(
                    Expression.Equal(handlersLength, one),
                    ifOneHandler,
                    elseManyHandlers
                ),
                resultTaskVar // <- Finally return the result
            );

            return Expression.Lambda<Func<IServiceProvider, object, CancellationToken, Task>>(
                block,
                spParam,
                eventParam,
                cancellationTokenParam
            ).Compile();
        }

    }
}
