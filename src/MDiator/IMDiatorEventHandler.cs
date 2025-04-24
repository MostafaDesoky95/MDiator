
namespace MDiator
{
    public interface IMDiatorEventHandler<TEvent>
        where TEvent : IMDiatorEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
