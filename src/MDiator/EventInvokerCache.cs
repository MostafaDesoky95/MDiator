using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MDiator
{
    internal static class EventInvokerCache
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object, Task>> _cache = new();

        public static Task Invoke(object handler, object @event)
        {
            var handlerType = handler.GetType();
            return _cache.GetOrAdd(handlerType, BuildInvoker)(handler, @event);
        }

        private static Func<object, object, Task> BuildInvoker(Type handlerType)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMDiatorEventHandler<>));

            var method = interfaceType.GetMethod("Handle");
            var eventType = interfaceType.GetGenericArguments()[0];

            var handlerParam = Expression.Parameter(typeof(object), "handler");
            var eventParam = Expression.Parameter(typeof(object), "event");

            var castedHandler = Expression.Convert(handlerParam, handlerType);
            var castedEvent = Expression.Convert(eventParam, eventType);

            var call = Expression.Call(castedHandler, method, castedEvent); // should return Task

            // Ensure the call result is cast to Task explicitly
            var castToTask = Expression.Convert(call, typeof(Task));

            var lambda = Expression.Lambda<Func<object, object, Task>>(castToTask, handlerParam, eventParam);
            return lambda.Compile();
        }
    }
}
