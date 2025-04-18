using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator
{
    public interface IMDiatorHandler<TRequest, TResponse>
          where TRequest : IMDiatorRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
