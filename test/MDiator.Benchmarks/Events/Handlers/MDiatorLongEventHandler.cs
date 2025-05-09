﻿namespace MDiator.Benchmarks.Events.Handlers;

public class MDiatorLongEventHandler : IMDiatorEventHandler<LongEvent>
{
    public async Task Handle(LongEvent notification, CancellationToken cancellationToken) => await Task.Delay(1000 * 15, cancellationToken);
}
