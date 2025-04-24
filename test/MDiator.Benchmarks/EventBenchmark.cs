using BenchmarkDotNet.Attributes;
using MDiator.Benchmarks.Events;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

[MemoryDiagnoser]
public class EventBenchmark
{
    private static readonly MediatR.IMediator _mediatR;
    private static readonly IMediator _mDiator;

    static EventBenchmark()
    {
        var services = new ServiceCollection();
        var assembly = typeof(RequestBenchmark).Assembly;
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
            .AddMDiator(assembly);

        var serviceProvider = services.BuildServiceProvider();

        _mediatR = serviceProvider.GetRequiredService<MediatR.IMediator>();
        _mDiator = serviceProvider.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public async Task MediatR_Publish() => await _mediatR.Publish(new MediatREvent());

    [Benchmark]
    public async Task MDiator_Publish() => await _mDiator.Publish(new MDiatorEvent());
}
