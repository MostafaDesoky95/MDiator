namespace MDiator.Benchmarks.Events.Handlers;

public class MDiatorShortEventHandler : IMDiatorEventHandler<ShortEvent>
{
    public async Task Handle(ShortEvent notification, CancellationToken cancellationToken) => await Task.CompletedTask;
}
