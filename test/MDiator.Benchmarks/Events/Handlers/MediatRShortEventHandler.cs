using MediatR;

namespace MDiator.Benchmarks.Events.Handlers;

public class MediatRShortEventHandler : INotificationHandler<ShortEvent>
{
    public async Task Handle(ShortEvent notification, CancellationToken cancellationToken) => await Task.CompletedTask;
}
