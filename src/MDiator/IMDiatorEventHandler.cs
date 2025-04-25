
namespace MDiator
{
    public interface IMDiatorEventHandler<TEvent>
        where TEvent : IMDiatorEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
