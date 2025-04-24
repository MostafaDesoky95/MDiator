namespace MDiator.Benchmarks.Events.Handlers;

public class MDiatorLongEventHandler : IMDiatorEventHandler<LongEvent>
{
    public async Task HandleAsync(LongEvent notification) => await Task.Delay(1000 * 15);
}
