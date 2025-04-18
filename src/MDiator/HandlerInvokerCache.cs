using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDiator
{
    internal static class HandlerInvokerCache
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object, Task<object>>> _cache = new();

        public static Task<object> Invoke(object handler, object request)
        {
            var handlerType = handler.GetType();
            return _cache.GetOrAdd(handlerType, BuildInvoker)(handler, request);
        }

        private static Func<object, object, Task<object>> BuildInvoker(Type handlerType)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMDiatorHandler<,>));

            var method = interfaceType.GetMethod("Handle");
            var requestType = interfaceType.GetGenericArguments()[0];
            var responseType = interfaceType.GetGenericArguments()[1];

            // Parameters: (object handler, object request)
            var handlerParam = Expression.Parameter(typeof(object), "handler");
            var requestParam = Expression.Parameter(typeof(object), "request");

            // Casts
            var castedHandler = Expression.Convert(handlerParam, handlerType);
            var castedRequest = Expression.Convert(requestParam, requestType);

            // Call handler.Handle(request)
            var call = Expression.Call(castedHandler, method, castedRequest);

            // Convert Task<T> to Task<object>
            var resultCast = Expression.Convert(call, typeof(object));
            var lambda = Expression.Lambda<Func<object, object, Task<object>>>(resultCast, handlerParam, requestParam);
            return lambda.Compile();
        }
    }
}
