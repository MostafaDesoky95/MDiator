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

        public async Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request)
        {
            var invoker = HandlerInvokerCache<TResponse>.Get(request.GetType());
            return await invoker(_provider, request);
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : IMDiatorEvent
        {
            return EventInvokerCache.Invoke(_provider, @event);
        }
    }
}