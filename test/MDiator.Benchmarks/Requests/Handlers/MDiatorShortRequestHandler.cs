namespace MDiator.Benchmarks.Requests.Handlers;

public class MDiatorShortRequestHandler : IMDiatorHandler<ShortRequest, string>
{
    public Task<string> Handle(ShortRequest request) => Task.FromResult("Short Pong");
}
