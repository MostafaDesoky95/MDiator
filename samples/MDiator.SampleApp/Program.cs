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
            var cts = new CancellationTokenSource();
            cts.CancelAfter(2000);
            var result = await mediator.Send(new CreateUserCommand { UserName = "Mostafa" }, cts.Token);
            Console.WriteLine(result);

            // ✅ Publish Event
            await mediator.Publish(new OrderCreatedEvent { OrderId = 123 });
        }
    }
}
