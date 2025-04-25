using MDiator.SampleApp.Events;

namespace MDiator.SampleApp.Handlers
{
    public class NotifyTeamHandler : IMDiatorEventHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent e, CancellationToken cancellationToken)
        {
            Console.WriteLine($"🔔 Team notified about Order #{e.OrderId}");
            return Task.CompletedTask;
        }
    }
}
