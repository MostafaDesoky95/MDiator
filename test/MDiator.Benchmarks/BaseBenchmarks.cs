using Microsoft.Extensions.DependencyInjection;

namespace MDiator.Benchmarks;

public abstract class BaseBenchmarks
{
    private static readonly ServiceProvider _serviceProvider;

    static BaseBenchmarks()
    {
        var services = new ServiceCollection();
        var assembly = typeof(BaseBenchmarks).Assembly;
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
            .AddMDiator(assembly);

        _serviceProvider = services.BuildServiceProvider();
    }

    protected ServiceProvider ServiceProvider => _serviceProvider;

    public abstract Task HandleEventAsync();

    public abstract Task HandleRequestAsync();
}
