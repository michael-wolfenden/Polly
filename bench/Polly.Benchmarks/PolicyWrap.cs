﻿namespace Polly.Benchmarks;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable

[Config(typeof(PollyConfig))]
public class PolicyWrap
{
    private static readonly Policy SyncPolicy = Policy.Wrap(
        Policy.Handle<InvalidOperationException>().Retry(),
        Policy.Handle<InvalidOperationException>().CircuitBreaker(2, TimeSpan.FromMinutes(1)),
        Policy.Timeout(TimeSpan.FromMilliseconds(10)),
        Policy.Bulkhead(2));

    private static readonly AsyncPolicy AsyncPolicy = Policy.WrapAsync(
        Policy.Handle<InvalidOperationException>().RetryAsync(),
        Policy.Handle<InvalidOperationException>().CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)),
        Policy.TimeoutAsync(TimeSpan.FromMilliseconds(10)),
        Policy.BulkheadAsync(2));

    [Benchmark]
    public static void PolicyWrap_Synchronous() =>
        SyncPolicy.Execute(() => Workloads.Action());

    [Benchmark]
    public static Task PolicyWrap_Asynchronous() =>
        AsyncPolicy.ExecuteAsync(token => Workloads.ActionAsync(token), CancellationToken.None);

    [Benchmark]
    public static int PolicyWrap_Synchronous_With_Result() =>
        SyncPolicy.Execute(() => Workloads.Func<int>());

    [Benchmark]
    public static Task<int> PolicyWrap_Asynchronous_With_Result() =>
        AsyncPolicy.ExecuteAsync(token => Workloads.FuncAsync<int>(token), CancellationToken.None);
}
