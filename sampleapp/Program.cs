using Microsoft.Extensions.DependencyInjection;

namespace MDiator.SampleApp
{
    public class CreateUserCommand : IMDiatorRequest<string>
    {
        public string UserName { get; set; }
    }

    public class CreateUserHandler : IMDiatorHandler<CreateUserCommand, string>
    {
        public Task<string> Handle(CreateUserCommand request)
        {
            return Task.FromResult($"✅ User '{request.UserName}' created.");
        }
    }

    // ======== EVENT ==========
    public class OrderCreatedEvent : IMDiatorEvent
    {
        public int OrderId { get; set; }
    }

    public class NotifyTeamHandler : IMDiatorEventHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent e)
        {
            Console.WriteLine($"🔔 Team notified about Order #{e.OrderId}");
            return Task.CompletedTask;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            // ✅ Setup DI
            var services = new ServiceCollection();
            services.AddMDiator(typeof(Program).Assembly); // auto-register handlers
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();

            // ✅ Send Command
            var result = await mediator.Send(new CreateUserCommand { UserName = "Mostafa" });
            Console.WriteLine(result);

            // ✅ Publish Event
            await mediator.Publish(new OrderCreatedEvent { OrderId = 123 });
        }
    }
}
