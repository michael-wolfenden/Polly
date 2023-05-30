using System.ComponentModel.DataAnnotations;
using Polly.Strategy;

namespace Polly;

/// <summary>
/// A builder that is used to create an instance of <see cref="ResilienceStrategy"/>.
/// </summary>
/// <remarks>
/// The builder supports chaining multiple strategies into a pipeline of strategies.
/// The resulting instance of <see cref="ResilienceStrategy"/> created by the <see cref="Build"/> call will execute the strategies in the same order they were added to the builder.
/// The order of the strategies is important.
/// </remarks>
public sealed class ResilienceStrategyBuilder : ResilienceStrategyBuilderBase
{
    /// <summary>
    /// Adds an already created strategy instance to the builder.
    /// </summary>
    /// <param name="strategy">The strategy instance.</param>
    /// <returns>The same builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="strategy"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this builder was already used to create a strategy. The builder cannot be modified after it has been used.</exception>
    public new ResilienceStrategyBuilder AddStrategy(ResilienceStrategy strategy)
    {
        Guard.NotNull(strategy);

        base.AddStrategy(strategy);
        return this;
    }

    /// <summary>
    /// Adds a strategy to the builder.
    /// </summary>
    /// <param name="factory">The factory that creates a resilience strategy.</param>
    /// <param name="options">The options associated with the strategy. If none are provided the default instance of <see cref="ResilienceStrategyOptions"/> is created.</param>
    /// <returns>The same builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this builder was already used to create a strategy. The builder cannot be modified after it has been used.</exception>
    /// <exception cref="ValidationException">Thrown when the <paramref name="options"/> are invalid.</exception>
    public new ResilienceStrategyBuilder AddStrategy(Func<ResilienceStrategyBuilderContext, ResilienceStrategy> factory, ResilienceStrategyOptions options)
    {
        base.AddStrategy(factory, options);
        return this;
    }

    /// <summary>
    /// Builds the resilience strategy.
    /// </summary>
    /// <returns>An instance of <see cref="ResilienceStrategy"/>.</returns>
    /// <exception cref="ValidationException">Thrown when this builder has invalid configuration.</exception>
    public ResilienceStrategy Build() => BuildStrategy();
}
