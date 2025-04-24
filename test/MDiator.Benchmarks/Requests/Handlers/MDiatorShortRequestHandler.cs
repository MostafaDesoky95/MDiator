namespace MDiator.Benchmarks.Requests.Handlers;

public class MDiatorShortRequestHandler : IMDiatorHandler<ShortRequest, string>
{
    public Task<string> HandleAsync(ShortRequest request) => Task.FromResult("Short Pong");
}
