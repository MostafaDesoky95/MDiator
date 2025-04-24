using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MDiator
{
    public static class EventInvokerCache
    {
        private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, object, Task>> _cache = new();

        public static Task Invoke(IServiceProvider provider, object @event)
        {
            var eventType = @event.GetType();
            var invoker = _cache.GetOrAdd(eventType, BuildInvoker);
            return invoker(provider, @event);
        }

        private static Func<IServiceProvider, object, Task> BuildInvoker(Type eventType)
        {
            var handlerType = typeof(IMDiatorEventHandler<>).MakeGenericType(eventType);
            var method = handlerType.GetMethod("Handle");

            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var eventParam = Expression.Parameter(typeof(object), "event");

            var getHandlers = Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetServices),
                new[] { handlerType },
                spParam
            );

            var handlersVar = Expression.Variable(typeof(IEnumerable<>).MakeGenericType(handlerType), "handlers");
            var loopVar = Expression.Variable(handlerType, "handler");

            var assignHandlers = Expression.Assign(handlersVar, getHandlers);

            var loop = handlersVar.ForEach(loopVar,
                Expression.Call(loopVar, method!, Expression.Convert(eventParam, eventType)));

            var block = Expression.Block(
                new[] { handlersVar },
                assignHandlers,
                loop,
                Expression.Constant(Task.CompletedTask)
            );

            return Expression.Lambda<Func<IServiceProvider, object, Task>>(
                block,
                spParam,
                eventParam
            ).Compile();
        }
    }

}
