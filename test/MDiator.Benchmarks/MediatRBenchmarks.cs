using MDiator.Benchmarks.Events;
using MDiator.Benchmarks.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

public class MediatRBenchmarks : BaseBenchmarks
{
    private readonly MediatR.IMediator _mediator;

    public MediatRBenchmarks()
        => _mediator = ServiceProvider.GetRequiredService<MediatR.IMediator>();

    public override async Task HandleEventAsync() => await _mediator.Publish(new MediatREvent());

    public override async Task HandleRequestAsync() => await _mediator.Send(new MediatRRequest());
}
