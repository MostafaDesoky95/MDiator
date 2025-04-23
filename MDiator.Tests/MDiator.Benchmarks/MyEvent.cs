using MDiator;
using MediatR;

public class MyEvent : INotification, IMDiatorEvent
{
    public string Message => "Event Triggered";
}

public class MyMDiatorEventHandler : IMDiatorEventHandler<MyEvent>
{
    public Task Handle(MyEvent notification)
    {
        // Simulate event handling logic
        return Task.CompletedTask;
    }
}

public class MyMediatREventHandler : INotificationHandler<MyEvent>
{
    public Task Handle(MyEvent notification, CancellationToken cancellationToken)
    {
        // Simulate event handling logic
        return Task.CompletedTask;
    }
}
