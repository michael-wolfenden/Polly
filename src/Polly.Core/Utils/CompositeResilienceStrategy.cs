using System.Diagnostics;

namespace Polly.Utils;

#pragma warning disable S2302 // "nameof" should be used

/// <summary>
/// A combination of multiple resilience strategies.
/// </summary>
[DebuggerDisplay("CompositeResilienceStrategy, Strategies = {Strategies.Count}")]
internal sealed partial class CompositeResilienceStrategy<T> : ResilienceStrategy<T>
{
    private readonly ResilienceStrategy<T> _firstStrategy;

    public static CompositeResilienceStrategy<T> Create(IReadOnlyList<ResilienceStrategy<T>> strategies)
    {
        Guard.NotNull(strategies);

        if (strategies.Count < 2)
        {
            throw new InvalidOperationException("The composite resilience strategy must contain at least two resilience strategies.");
        }

        if (strategies.Distinct().Count() != strategies.Count)
        {
            throw new InvalidOperationException("The composite resilience strategy must contain unique resilience strategies.");
        }

        // convert all strategies to delegating ones (except the last one as it's not required)
        var delegatingStrategies = strategies
            .Take(strategies.Count - 1)
            .Select(strategy => new DelegatingResilienceStrategy(strategy))
            .ToList();

#if NET6_0_OR_GREATER
        // link the last one
        delegatingStrategies[^1].Next = strategies[^1];
#else
        delegatingStrategies[delegatingStrategies.Count - 1].Next = strategies[strategies.Count - 1];
#endif

        // link the remaining ones
        for (var i = 0; i < delegatingStrategies.Count - 1; i++)
        {
            delegatingStrategies[i].Next = delegatingStrategies[i + 1];
        }

        return new CompositeResilienceStrategy<T>(delegatingStrategies[0], strategies);
    }

    private CompositeResilienceStrategy(ResilienceStrategy<T> first, IReadOnlyList<ResilienceStrategy<T>> strategies)
    {
        Strategies = strategies;
        _firstStrategy = first;
    }

    public IReadOnlyList<ResilienceStrategy<T>> Strategies { get; }

    protected override ValueTask<Outcome<T>> ExecuteCore<TState>(
        Func<ResilienceContext, TState, ValueTask<Outcome<T>>> callback,
        ResilienceContext context, TState state)
    {
        if (context.CancellationToken.IsCancellationRequested)
        {
            return Outcome.FromExceptionAsTask<T>(new OperationCanceledException(context.CancellationToken).TrySetStackTrace());
        }

        return _firstStrategy.ExecuteCoreAsync(callback, context, state);
    }

    /// <summary>
    /// A resilience strategy that delegates the execution to the next strategy in the chain.
    /// </summary>
    private sealed class DelegatingResilienceStrategy : ResilienceStrategy<T>
    {
        private readonly ResilienceStrategy<T> _strategy;

        public DelegatingResilienceStrategy(ResilienceStrategy<T> strategy) => _strategy = strategy;

        public ResilienceStrategy<T>? Next { get; set; }

        protected override ValueTask<Outcome<T>> ExecuteCore<TState>(
            Func<ResilienceContext, TState, ValueTask<Outcome<T>>> callback,
            ResilienceContext context,
            TState state)
        {
            return _strategy.ExecuteCoreAsync(
                static (context, state) =>
                {
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        return Outcome.FromExceptionAsTask<T>(new OperationCanceledException(context.CancellationToken).TrySetStackTrace());
                    }

                    return state.Next!.ExecuteCoreAsync(state.callback, context, state.state);
                },
                context,
                (Next, callback, state));
        }
    }
}
