using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IMDiatorEvent;
    }
}
