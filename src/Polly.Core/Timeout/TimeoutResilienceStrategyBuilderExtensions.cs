using System;
using Polly.Builder;

namespace Polly.Timeout;

/// <summary>
/// Extension methods for adding timeouts to a <see cref="ResilienceStrategyBuilder"/>.
/// </summary>
public static class TimeoutResilienceStrategyBuilderExtensions
{
    /// <summary>
    /// Adds a timeout resilience strategy to the builder.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="timeout">The timeout value. This value should be greater than <see cref="TimeSpan.Zero"/> and less than <see cref="System.Threading.Timeout.InfiniteTimeSpan"/>.</param>
    /// <returns>The same builder instance.</returns>
    public static ResilienceStrategyBuilder AddTimeout(this ResilienceStrategyBuilder builder, TimeSpan timeout)
    {
        Guard.NotNull(builder);
        TimeoutUtil.ValidateTimeout(timeout);

        return builder.AddTimeout(new TimeoutStrategyOptions
        {
            TimeoutGenerator = new TimeoutGenerator(timeout)
        });
    }

    /// <summary>
    /// Adds a timeout resilience strategy to the builder.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="timeout">The timeout value. This value should be greater than <see cref="TimeSpan.Zero"/> and less than <see cref="System.Threading.Timeout.InfiniteTimeSpan"/>.</param>
    /// <param name="onTimeout">The callback that is executed when timeout happens.</param>
    /// <returns>The same builder instance.</returns>
    public static ResilienceStrategyBuilder AddTimeout(this ResilienceStrategyBuilder builder, TimeSpan timeout, Action<OnTimeoutArguments> onTimeout)
    {
        Guard.NotNull(builder);
        Guard.NotNull(onTimeout);
        TimeoutUtil.ValidateTimeout(timeout);

        return builder.AddTimeout(new TimeoutStrategyOptions
        {
            TimeoutGenerator = new TimeoutGenerator(timeout),
            OnTimeout = new OnTimeoutEvent().Add(onTimeout),
        });
    }

    /// <summary>
    /// Adds a timeout resilience strategy to the builder.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="options">The timeout options.</param>
    /// <returns>The same builder instance.</returns>
    public static ResilienceStrategyBuilder AddTimeout(this ResilienceStrategyBuilder builder, TimeoutStrategyOptions options)
    {
        Guard.NotNull(builder);
        Guard.NotNull(options);

        ValidationHelper.ValidateObject(options, "The timeout strategy options are invalid.");

        return builder.AddStrategy(context => new TimeoutResilienceStrategy(options, context.TimeProvider, context.Telemetry), options);
    }
}
