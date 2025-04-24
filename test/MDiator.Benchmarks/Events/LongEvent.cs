using MediatR;

namespace MDiator.Benchmarks.Events;

public class LongEvent : IMDiatorEvent, INotification
{
    public string Message => "Long Event";
}
