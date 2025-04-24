using MediatR;

namespace MDiator.Benchmarks.Events;

public class MediatREvent : INotification
{
    public string Message => "Event Triggered";
}
