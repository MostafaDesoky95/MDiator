using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IMDiatorRequest<TResponse> request);
        Task Publish<TEvent>(TEvent @event) where TEvent : IMDiatorEvent;
    }
}
