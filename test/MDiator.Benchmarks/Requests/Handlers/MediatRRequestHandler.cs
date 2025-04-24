using MediatR;

namespace MDiator.Benchmarks.Requests.Handlers;

public class MediatRRequestHandler : IRequestHandler<MediatRRequest, string>
{
    public Task<string> Handle(MediatRRequest request, CancellationToken cancellationToken) => Task.FromResult("Pong");
}