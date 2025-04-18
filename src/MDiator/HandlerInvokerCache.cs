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

            var requestType = interfaceType.GetGenericArguments()[0];
            var responseType = interfaceType.GetGenericArguments()[1];

            // Parameters
            var handlerParam = Expression.Parameter(typeof(object), "handler");
            var requestParam = Expression.Parameter(typeof(object), "request");

            // Casts
            var castedHandler = Expression.Convert(handlerParam, handlerType);
            var castedRequest = Expression.Convert(requestParam, requestType);

            // Method call: handler.Handle(request)
            var call = Expression.Call(
                castedHandler,
                handlerType.GetMethod("Handle"),
                castedRequest
            );

            // Convert Task<T> → Task<object>
            var funcType = typeof(Func<,,>).MakeGenericType(typeof(object), typeof(object), typeof(Task<object>));

            var wrapper = typeof(HandlerInvokerCache)
                .GetMethod(nameof(WrapTask), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .MakeGenericMethod(responseType);

            var lambda = Expression.Lambda<Func<object, object, Task<object>>>(
                Expression.Call(wrapper, call),
                handlerParam,
                requestParam
            );

            return lambda.Compile();
        }

        // Helper to wrap Task<T> → Task<object> with await and unboxing
        private static async Task<object> WrapTask<T>(Task<T> task)
        {
            return await task.ConfigureAwait(false);
        }
    }
}
