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

        public async Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var invoker = HandlerInvokerCache<TResponse>.Get(request.GetType());
            return await invoker(_provider, request, cancellationToken);
        }

        public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IMDiatorEvent
        {
            return EventInvokerCache.Invoke(_provider, @event, cancellationToken);
        }
    }
}