using MDiator.Tests.Handlers;
using MDiator.Tests.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Tests
{
    public class RequestTests
    {
        [Fact]
        public async Task Send_Should_Invoke_Handler_And_Return_Result()
        {
            var services = new ServiceCollection();
            services.AddMDiator(typeof(MyRequest).Assembly);
            services.AddScoped<IMDiatorHandler<MyRequest, string>, MyHandler>();

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();
            var cancellationToken = new CancellationTokenSource().Token;
            var result = await mediator.Send(new MyRequest(), cancellationToken);

            Assert.Equal("Handled: Test", result);
        }
    }
}
