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
            var handlerType = typeof(IMDiatorEventHandler<>).MakeGenericType(eventType);
            var handleMethod = handlerType.GetMethod("Handle");

            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var eventParam = Expression.Parameter(typeof(object), "event");
            var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

            var getHandlers = Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetServices),
                new[] { handlerType },
                spParam
            );

            var handlersVar = Expression.Variable(typeof(IEnumerable<>).MakeGenericType(handlerType), "handlers");
            var handlerVar = Expression.Variable(handlerType, "handler");

            var tasksVar = Expression.Variable(typeof(List<Task>), "tasks");

      


            var assignTasks = Expression.Assign(tasksVar, Expression.New(typeof(List<Task>)));

            var assignHandlers = Expression.Assign(handlersVar, getHandlers);


            var callHandle = Expression.Call(
                  handlerVar,
                  handleMethod,
                  Expression.Convert(eventParam, eventType),
                  cancellationTokenParam
              );

            var addTaskToList = Expression.Call(
                tasksVar,
                typeof(List<Task>).GetMethod(nameof(List<Task>.Add))!,
                callHandle
            );

            var loop = handlersVar.ForEach(handlerVar, addTaskToList);

            var whenAllCall = Expression.Call(
                typeof(Task),
                nameof(Task.WhenAll),
                Type.EmptyTypes,
                tasksVar
            );
            
            var block = Expression.Block(
                 new[] { handlersVar, tasksVar },
                 assignHandlers,
                 assignTasks,
                 loop,
                 whenAllCall
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
