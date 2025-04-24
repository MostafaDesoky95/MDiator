using MediatR;

namespace MDiator.Benchmarks.Events.Handlers;

public class MediatREventHandler : INotificationHandler<MediatREvent>
{
    public Task Handle(MediatREvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
}
