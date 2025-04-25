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
        public async Task<string> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(10000, cancellationToken); // Simulate some async work
            return await Task.FromResult($"Handled: {request.Message}");
        }
    }
}
