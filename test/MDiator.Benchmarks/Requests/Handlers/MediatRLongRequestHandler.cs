using MediatR;

namespace MDiator.Benchmarks.Requests.Handlers;

public class MediatRLongRequestHandler : IRequestHandler<LongRequest, string>
{
    public async Task<string> Handle(LongRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000 * 15, cancellationToken);

        return await Task.FromResult("Long Pong");
    }
}