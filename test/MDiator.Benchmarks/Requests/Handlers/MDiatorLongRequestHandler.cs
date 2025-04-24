namespace MDiator.Benchmarks.Requests.Handlers;

public class MDiatorLongRequestHandler : IMDiatorHandler<LongRequest, string>
{
    public async Task<string> HandleAsync(LongRequest request)
    {
        await Task.Delay(1000 * 15);

        return await Task.FromResult("Long Pong");
    }
}
