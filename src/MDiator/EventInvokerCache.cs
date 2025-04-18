using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace MDiator
{
    internal static class EventInvokerCache
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object, Task>> _cache = new();

        public static Task Invoke(object handler, object @event)
        {
            var type = handler.GetType();
            return _cache.GetOrAdd(type, BuildInvoker)(handler, @event);
        }

        private static Func<object, object, Task> BuildInvoker(Type handlerType)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMDiatorEventHandler<>));

            var method = interfaceType.GetMethod("Handle");
            var eventType = interfaceType.GetGenericArguments()[0];

            // Parameters
            var handlerParam = Expression.Parameter(typeof(object), "handler");
            var eventParam = Expression.Parameter(typeof(object), "event");

            // Casts
            var castedHandler = Expression.Convert(handlerParam, handlerType);
            var castedEvent = Expression.Convert(eventParam, eventType);

            // Call: handler.Handle(@event)
            var call = Expression.Call(castedHandler, method, castedEvent);

            return Expression.Lambda<Func<object, object, Task>>(call, handlerParam, eventParam).Compile();
        }
    }
}
