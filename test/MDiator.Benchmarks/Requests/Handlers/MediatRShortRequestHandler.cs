using MediatR;

namespace MDiator.Benchmarks.Requests.Handlers;

public class MediatRShortRequestHandler : IRequestHandler<ShortRequest, string>
{
    public Task<string> Handle(ShortRequest request, CancellationToken cancellationToken) => Task.FromResult("Short Pong");
}