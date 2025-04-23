using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RequestBenchmark>();
BenchmarkRunner.Run<EventBenchmark>();