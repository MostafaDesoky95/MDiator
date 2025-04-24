using BenchmarkDotNet.Running;
using MDiator.Benchmarks;

BenchmarkRunner.Run<RequestBenchmark>();
BenchmarkRunner.Run<EventBenchmark>();