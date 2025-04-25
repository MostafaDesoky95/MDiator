using MDiator;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MDiator
{
    public static class HandlerInvokerCache<TResponse>
    {
        private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, IMDiatorRequest<TResponse>, CancellationToken, Task<TResponse>>> _cache = new();

        public static Func<IServiceProvider, IMDiatorRequest<TResponse>, CancellationToken, Task<TResponse>> Get(Type requestType)
        {
            return _cache.GetOrAdd(requestType, BuildInvoker);
        }

        private static Func<IServiceProvider, IMDiatorRequest<TResponse>, CancellationToken, Task<TResponse>> BuildInvoker(Type requestType)
        {
            var handlerInterface = typeof(IMDiatorHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handleMethod = handlerInterface.GetMethod("Handle");

            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var requestParam = Expression.Parameter(typeof(IMDiatorRequest<TResponse>), "request");
            var cancellationTokenParam = Expression.Parameter(typeof(CancellationToken), "cancellationToken");

            // sp.GetRequiredService<IMDiatorHandler<TRequest, TResponse>>()
            var getHandlerCall = Expression.Call(
                typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] { handlerInterface },
                spParam
            );

            var call = Expression.Call(
                Expression.Convert(getHandlerCall, handlerInterface),
                handleMethod!,
                Expression.Convert(requestParam, requestType),
                cancellationTokenParam
            );

            var lambda = Expression.Lambda<Func<IServiceProvider, IMDiatorRequest<TResponse>, CancellationToken, Task<TResponse>>>(
                call,
                spParam,
                requestParam,
                cancellationTokenParam
            );

            return lambda.Compile();
        }
    }
}