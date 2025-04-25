using MDiator.Extensions;
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
            var method = handlerType.GetMethod("Handle");

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
            var loopVar = Expression.Variable(handlerType, "handler");

            var assignHandlers = Expression.Assign(handlersVar, getHandlers);

            var loop = ExpressionEx.ForEach(
                handlersVar,
                loopVar,
                Expression.Call(loopVar, method!, Expression.Convert(eventParam, eventType), cancellationTokenParam)
            );

            var block = Expression.Block(
                new[] { handlersVar },
                assignHandlers,
                loop,
                Expression.Constant(Task.CompletedTask)
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
