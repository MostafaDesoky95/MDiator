using MDiator.Benchmarks.Events;
using MDiator.Benchmarks.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

public class MDiatorBenchmarks : BaseBenchmarks
{
    private readonly IMediator _mediator;

    public MDiatorBenchmarks()
        => _mediator = ServiceProvider.GetRequiredService<IMediator>();

    public override async Task HandleEventAsync() => await _mediator.Publish(new MDiatorEvent());

    public override async Task HandleRequestAsync() => await _mediator.Send(new MDiatorRequest());
}
