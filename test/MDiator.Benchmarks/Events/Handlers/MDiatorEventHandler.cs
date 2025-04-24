namespace MDiator.Benchmarks.Events.Handlers;

public class MDiatorEventHandler : IMDiatorEventHandler<MDiatorEvent>
{
    public Task Handle(MDiatorEvent notification) => Task.CompletedTask;
}
