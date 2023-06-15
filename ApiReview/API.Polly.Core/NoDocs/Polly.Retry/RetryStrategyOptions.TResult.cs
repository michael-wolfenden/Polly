// Assembly 'Polly.Core'

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Polly.Retry;

public class RetryStrategyOptions<TResult> : ResilienceStrategyOptions
{
    public sealed override string StrategyType { get; }
    [Range(-1, 100)]
    public int RetryCount { get; set; }
    public RetryBackoffType BackoffType { get; set; }
    [TimeSpan("00:00:00", "1.00:00:00")]
    public TimeSpan BaseDelay { get; set; }
    [Required]
    public Func<OutcomeArguments<TResult, ShouldRetryArguments>, ValueTask<bool>>? ShouldRetry { get; set; }
    public Func<OutcomeArguments<TResult, RetryDelayArguments>, ValueTask<TimeSpan>>? RetryDelayGenerator { get; set; }
    public Func<OutcomeArguments<TResult, OnRetryArguments>, ValueTask>? OnRetry { get; set; }
    public RetryStrategyOptions();
}
