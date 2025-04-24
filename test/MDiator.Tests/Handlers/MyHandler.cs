using MDiator.Tests.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator.Tests.Handlers
{
    public class MyHandler : IMDiatorHandler<MyRequest, string>
    {
        public Task<string> HandleAsync(MyRequest request)
        {
            return Task.FromResult($"Handled: {request.Message}");
        }
    }
}
