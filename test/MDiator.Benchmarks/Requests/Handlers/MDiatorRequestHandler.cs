namespace MDiator.Benchmarks.Requests.Handlers;

public class MDiatorRequestHandler : IMDiatorHandler<MDiatorRequest, string>
{
    public Task<string> Handle(MDiatorRequest request) => Task.FromResult("Pong");
}
