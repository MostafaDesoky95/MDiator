using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

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
            var result = await HandlerInvokerCache.Invoke(_provider, request, typeof(TResponse));
            return (TResponse)result;
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : IMDiatorEvent
        {
            return EventInvokerCache.Invoke(_provider, @event);
        }
    }
}