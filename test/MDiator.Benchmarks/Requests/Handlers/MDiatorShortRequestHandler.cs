namespace MDiator.Benchmarks.Requests.Handlers;

public class MDiatorShortRequestHandler : IMDiatorHandler<ShortRequest, string>
{
    public Task<string> Handle(ShortRequest request, CancellationToken cancellationToken) => Task.FromResult("Short Pong");
}
