using MDiator;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MDiator
{
    public static class HandlerInvokerCache
    {
        private static readonly ConcurrentDictionary<(Type RequestType, Type ResponseType), Func<IServiceProvider, object, Task<object>>> _cache = new();

        public static Task<object> Invoke(IServiceProvider provider, object request, Type responseType)
        {
            var key = (request.GetType(), responseType);
            var invoker = _cache.GetOrAdd(key, BuildInvoker);
            return invoker(provider, request);
        }

        private static Func<IServiceProvider, object, Task<object>> BuildInvoker((Type requestType, Type responseType) key)
        {
            var (requestType, responseType) = key;

            var handlerType = typeof(IMDiatorHandler<,>).MakeGenericType(requestType, responseType);
            var handleMethod = handlerType.GetMethod("Handle");

            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var requestParam = Expression.Parameter(typeof(object), "request");

            // handler = (IMDiatorHandler<,>)sp.GetRequiredService(...)
            var getHandler = Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] { handlerType },
                spParam
            );

            // ((TRequest)request)
            var castRequest = Expression.Convert(requestParam, requestType);

            // handler.Handle((TRequest)request)
            var call = Expression.Call(Expression.Convert(getHandler, handlerType), handleMethod!, castRequest);

            // wrap to Task<object>
            var wrap = typeof(HandlerInvokerCache)
                .GetMethod(nameof(WrapTask), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(responseType);

            var wrappedCall = Expression.Call(wrap, call);

            return Expression.Lambda<Func<IServiceProvider, object, Task<object>>>(
                wrappedCall,
                spParam,
                requestParam
            ).Compile();
        }

        private static async Task<object> WrapTask<T>(Task<T> task)
        {
            return await task.ConfigureAwait(false);
        }
    }
}