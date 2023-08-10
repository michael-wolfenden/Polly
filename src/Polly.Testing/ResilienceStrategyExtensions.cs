﻿using Polly.Utils;

namespace Polly.Testing;

/// <summary>
/// The test-related extensions for <see cref="ResilienceStrategy"/> and <see cref="ResilienceStrategy{TResult}"/>.
/// </summary>
public static class ResilienceStrategyExtensions
{
    /// <summary>
    /// Gets the inner strategies the <paramref name="strategy"/> is composed of.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="strategy">The strategy instance.</param>
    /// <returns>A list of inner strategies.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="strategy"/> is <see langword="null"/>.</exception>
    public static InnerStrategiesDescriptor GetInnerStrategies<TResult>(this ResilienceStrategy<TResult> strategy)
    {
        Guard.NotNull(strategy);

        return GetInnerStrategiesCore<TResult>(strategy.Strategy);
    }

    /// <summary>
    /// Gets the inner strategies the <paramref name="strategy"/> is composed of.
    /// </summary>
    /// <param name="strategy">The strategy instance.</param>
    /// <returns>A list of inner strategies.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="strategy"/> is <see langword="null"/>.</exception>
    public static InnerStrategiesDescriptor GetInnerStrategies(this ResilienceStrategy strategy)
    {
        Guard.NotNull(strategy);

        return GetInnerStrategiesCore<object>(strategy);
    }

    private static InnerStrategiesDescriptor GetInnerStrategiesCore<T>(ResilienceStrategy strategy)
    {
        var strategies = new List<ResilienceStrategy>();
        strategy.ExpandStrategies(strategies);

        var innerStrategies = strategies.Select(s => new ResilienceStrategyDescriptor(s.Options, GetStrategyInstance<T>(s))).ToList();

        return new InnerStrategiesDescriptor(
            innerStrategies.Where(s => !ShouldSkip(s.StrategyInstance)).ToList().AsReadOnly(),
            isReloadable: innerStrategies.Exists(s => s.StrategyInstance is ReloadableResilienceStrategy));
    }

    private static object GetStrategyInstance<T>(ResilienceStrategy strategy)
    {
        if (strategy is ReactiveResilienceStrategyBridge<T> reactiveBridge)
        {
            return reactiveBridge.Strategy;
        }

        if (strategy is NonReactiveResilienceStrategyBridge nonReactiveBridge)
        {
            return nonReactiveBridge.Strategy;
        }

        return strategy;
    }

    private static bool ShouldSkip(object instance) => instance is ReloadableResilienceStrategy;

    private static void ExpandStrategies(this ResilienceStrategy strategy, List<ResilienceStrategy> strategies)
    {
        if (strategy is CompositeResilienceStrategy pipeline)
        {
            foreach (var inner in pipeline.Strategies)
            {
                inner.ExpandStrategies(strategies);
            }
        }
        else if (strategy is ReloadableResilienceStrategy reloadable)
        {
            strategies.Add(reloadable);
            ExpandStrategies(reloadable.Strategy, strategies);
        }
        else
        {
            strategies.Add(strategy);
        }
    }
}
