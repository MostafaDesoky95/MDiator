using MDiator.Benchmarks.Events;
using MDiator.Benchmarks.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

public class MDiatorBenchmarks : BaseBenchmarks
{
    private readonly IMediator _mediator;

    public MDiatorBenchmarks()
        => _mediator = ServiceProvider.GetRequiredService<IMediator>();

    public override async Task HandleEventAsync() => await _mediator.Publish(new ShortEvent());

    public override async Task HandleLongEventAsync() => await _mediator.Publish(new LongEvent());

    public override async Task HandleRequestAsync() => await _mediator.Send(new ShortRequest());

    public override async Task HandleLongRequestAsync() => await _mediator.Send(new LongRequest());
}
