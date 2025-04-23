using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RequestBenchmark>();
//BenchmarkRunner.Run<EventBenchmark>();


//using BenchmarkDotNet.Attributes;
//using BenchmarkDotNet.Running;
//using Microsoft.Extensions.DependencyInjection;
//using MDiator;

//// ✅ Minimal Request
//public class Ping : IMDiatorRequest<string> { }

//// ✅ Minimal Handler
//public class PingHandler : IMDiatorHandler<Ping, string>
//{
//    public Task<string> Handle(Ping request) => Task.FromResult("pong");
//}

//// ✅ Benchmark Class
//[MemoryDiagnoser]
//public class CoreDelegateBenchmark
//{
//    private IMediator _mediator;
//    private Ping _request = new();

//    [GlobalSetup]
//    public void Setup()
//    {
//        var services = new ServiceCollection();
//        services.AddMDiator(typeof(Ping).Assembly);
//        _mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();
//    }

//    [Benchmark]
//    public async Task<string> MDiator_Send()
//    {
//        return await _mediator.Send(_request);
//    }

//    [Benchmark]
//    public async Task<object> MDiator_Core_Invoke()
//    {
//        var handler = new PingHandler();
//        return await HandlerInvokerCache.Invoke(handler, _request);
//    }
//}

//// ✅ Entry point
//public class Program
//{
//    public static void Main(string[] args)
//        => BenchmarkRunner.Run<CoreDelegateBenchmark>();
//}