using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace MDiator.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[ShortRunJob]
public class ComparisonBenchmarks
{
    private readonly MDiatorBenchmarks _mDiatorBenchmarks = new();
    private readonly MediatRBenchmarks _mediatRBenchmarks = new();

    //[Benchmark(Baseline = true), BenchmarkCategory("HandleEvent")]
    //public async Task MDiator_HandleEventAsync() => await _mDiatorBenchmarks.HandleEventAsync();

    //[Benchmark, BenchmarkCategory("HandleEvent")]
    //public async Task MediatR_HandleEventAsync() => await _mediatRBenchmarks.HandleEventAsync();

    [Benchmark(Baseline = true), BenchmarkCategory("HandleLongEvent")]
    public async Task MDiator_HandleEventLongAsync() => await _mDiatorBenchmarks.HandleLongEventAsync();

    //[Benchmark, BenchmarkCategory("HandleLongEvent")]
    //public async Task MediatR_HandleEventLongAsync() => await _mediatRBenchmarks.HandleLongEventAsync();

    //[Benchmark(Baseline = true), BenchmarkCategory("HandleRequest")]
    //public async Task MDiator_HandleRequestAsync() => await _mDiatorBenchmarks.HandleRequestAsync();

    //[Benchmark, BenchmarkCategory("HandleRequest")]
    //public async Task MediatR_HandleRequestAsync() => await _mediatRBenchmarks.HandleRequestAsync();

    //[Benchmark(Baseline = true), BenchmarkCategory("HandleLongRequest")]
    //public async Task MDiator_HandleLongRequestAsync() => await _mDiatorBenchmarks.HandleLongRequestAsync();

    //[Benchmark, BenchmarkCategory("HandleLongRequest")]
    //public async Task MediatR_HandleLongRequestAsync() => await _mediatRBenchmarks.HandleLongRequestAsync();
}
