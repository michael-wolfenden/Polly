```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
12th Gen Intel Core i7-1280P, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.101
  [Host] : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX2

Job=MediumRun  Toolchain=InProcessEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
| Method                         | Mean     | Error    | StdDev   | Allocated |
|------------------------------- |---------:|---------:|---------:|----------:|
| CompositeComponent_ExecuteCore | 40.70 ns | 0.574 ns | 0.823 ns |         - |
