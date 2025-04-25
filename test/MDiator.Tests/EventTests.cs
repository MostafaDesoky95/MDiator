using MDiator.Tests.Events;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDiator.Tests
{
    public class EventTests
    {
        [Fact]
        public async Task Publish_Should_Invoke_All_EventHandlers()
        {
            var mock1 = new Mock<IMDiatorEventHandler<MyEvent>>();
            var mock2 = new Mock<IMDiatorEventHandler<MyEvent>>();

            var cancellationToken = new CancellationTokenSource();

            var services = new ServiceCollection();
            services.AddMDiator(typeof(MyEvent).Assembly);
            services.AddSingleton(mock1.Object);
            services.AddSingleton(mock2.Object);

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await mediator.Publish(new MyEvent());

            mock1.Verify(m => m.Handle(It.IsAny<MyEvent>(), cancellationToken.Token), Times.Once);
            mock2.Verify(m => m.Handle(It.IsAny<MyEvent>(), cancellationToken.Token), Times.Once);
        }
    }
}
