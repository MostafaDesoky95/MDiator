namespace MDiator.Benchmarks.Events.Handlers;

public class MDiatorShortEventHandler : IMDiatorEventHandler<ShortEvent>
{
    public async Task HandleAsync(ShortEvent notification) => await Task.CompletedTask;
}
