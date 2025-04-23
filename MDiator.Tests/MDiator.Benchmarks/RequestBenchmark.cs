using BenchmarkDotNet.Attributes;
using MediatR;
using MDiator;
using Microsoft.Extensions.DependencyInjection;

[MemoryDiagnoser]
public class RequestBenchmark
{
    private MediatR.IMediator mediatR;
    private MDiator.IMediator mDiator;

    [GlobalSetup]
    public void Setup()
    {
        // MediatR setup
        var services1 = new ServiceCollection();
        services1.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MyRequest).Assembly));
        var serviceProvider1 = services1.BuildServiceProvider();
        mediatR = serviceProvider1.GetRequiredService<MediatR.IMediator>();

        // MDiator setup
        var services2 = new ServiceCollection();
        services2.AddMDiator(typeof(MyRequest).Assembly);
        var serviceProvider2 = services2.BuildServiceProvider();
        mDiator = serviceProvider2.GetRequiredService<MDiator.IMediator>();
    }

    [Benchmark]
    public async Task<string> MDiator_Send()
    {
        return await mDiator.Send(new MyRequest());
    }

    [Benchmark]
    public async Task<string> MediatR_Send()
    {
        return await mediatR.Send(new MyRequest());
    }
}
