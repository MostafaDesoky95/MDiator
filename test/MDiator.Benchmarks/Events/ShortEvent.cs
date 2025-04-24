using MediatR;

namespace MDiator.Benchmarks.Events;

public class ShortEvent : IMDiatorEvent, INotification
{
    public string Message => "Short Event";
}
