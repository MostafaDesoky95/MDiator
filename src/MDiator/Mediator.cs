using Microsoft.Extensions.DependencyInjection;

namespace MDiator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider provider)
        {
            _provider = provider;
        }


        // Use the commented code if you want to use reflection to invoke the handler
        //public async Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request)
        //{
        //    var handlerType = typeof(IMDiatorHandler<,>)
        //        .MakeGenericType(request.GetType(), typeof(TResponse));

        //    var handler = _provider.GetService(handlerType);
        //    if (handler == null)
        //        throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

        //    var method = handlerType.GetMethod("Handle");
        //    return await (Task<TResponse>)method.Invoke(handler, new object[] { request });
        //}



        // Use the HandlerInvokerCache to invoke the handler
        public async Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request)
        {
            var handlerType = typeof(IMDiatorHandler<,>)
                .MakeGenericType(request.GetType(), typeof(TResponse));

            var handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new InvalidOperationException($"Handler not found for {request.GetType().Name}");

            var result = await HandlerInvokerCache.Invoke(handler, request);
            return (TResponse)result;
        }



        // Use the commented code if you want to use reflection to invoke the event handler
        //public async Task Publish<TEvent>(TEvent @event) where TEvent : IMDiatorEvent
        //{
        //    var handlerType = typeof(IMDiatorEventHandler<>).MakeGenericType(@event.GetType());
        //    var handlers = _provider.GetServices(handlerType);

        //    foreach (var handler in handlers)
        //    {
        //        var method = handlerType.GetMethod("Handle");
        //        var task = (Task)method.Invoke(handler, new object[] { @event });
        //        await task;
        //    }
        //}


        // Use the EventInvokerCache to invoke the event handler
        public async Task Publish<TEvent>(TEvent @event) where TEvent : IMDiatorEvent
        {
            var handlerType = typeof(IMDiatorEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                await EventInvokerCache.Invoke(handler, @event);
            }
        }

    }
}