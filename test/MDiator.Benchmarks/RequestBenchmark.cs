using BenchmarkDotNet.Attributes;
using MDiator.Benchmarks.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

[MemoryDiagnoser]
public class RequestBenchmark
{
    private static readonly MediatR.IMediator _mediatR;
    private static readonly IMediator _mDiator;

    static RequestBenchmark()
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
    public async Task<string> MDiator_Send() => await _mDiator.Send(new MDiatorRequest());

    [Benchmark]
    public async Task<string> MediatR_Send() => await _mediatR.Send(new MediatRRequest());
}
