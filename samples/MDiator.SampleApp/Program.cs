using MDiator.SampleApp.Events;
using MDiator.SampleApp.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.SampleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // ✅ Setup DI
            var services = new ServiceCollection();
            services.AddMDiator(typeof(Program).Assembly);
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();

            // ✅ Send Command
            var result = await mediator.SendAsync(new CreateUserCommand { UserName = "Mostafa" });
            Console.WriteLine(result);

            // ✅ Publish Event
            await mediator.PublishAsync(new OrderCreatedEvent { OrderId = 123 });
        }
    }
}
