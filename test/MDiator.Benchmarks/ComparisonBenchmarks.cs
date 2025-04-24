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

    [Benchmark(Baseline = true), BenchmarkCategory("HandleEvent")]
    public async Task MDiator_HandleEventAsync() => await _mDiatorBenchmarks.HandleEventAsync();

    [Benchmark, BenchmarkCategory("HandleEvent")]
    public async Task MediatR_HandleEventAsync() => await _mediatRBenchmarks.HandleEventAsync();

    [Benchmark(Baseline = true), BenchmarkCategory("HandleRequest")]
    public async Task MDiator_HandleRequestAsync() => await _mDiatorBenchmarks.HandleRequestAsync();

    [Benchmark, BenchmarkCategory("HandleRequest")]
    public async Task MediatR_HandleRequestAsync() => await _mediatRBenchmarks.HandleRequestAsync();
}
