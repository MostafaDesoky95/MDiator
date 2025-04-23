using MediatR;
using MDiator;
using System.Threading;
using System.Threading.Tasks;

public class MyRequest : IMDiatorRequest<string>, IRequest<string>
{
    public string Payload => "Ping";
}

public class MyMDiatorHandler : IMDiatorHandler<MyRequest, string>
{
    public Task<string> Handle(MyRequest request) => Task.FromResult("Pong");
}

public class MyMediatRHandler : IRequestHandler<MyRequest, string>
{
    public Task<string> Handle(MyRequest request, CancellationToken cancellationToken) => Task.FromResult("Pong");
}