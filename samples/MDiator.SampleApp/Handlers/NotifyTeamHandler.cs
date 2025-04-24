using MDiator.SampleApp.Events;

namespace MDiator.SampleApp.Handlers
{
    public class NotifyTeamHandler : IMDiatorEventHandler<OrderCreatedEvent>
    {
        public Task HandleAsync(OrderCreatedEvent e)
        {
            Console.WriteLine($"🔔 Team notified about Order #{e.OrderId}");
            return Task.CompletedTask;
        }
    }
}
