﻿using System.ComponentModel.DataAnnotations;
using Polly.Builder;

namespace Polly.Registry;

/// <summary>
/// An options class used by <see cref="ResilienceStrategyRegistry{TKey}"/>.
/// </summary>
/// <typeparam name="TKey">The type of the key used by the registry.</typeparam>
public class ResilienceStrategyRegistryOptions<TKey>
{
    /// <summary>
    /// Gets or sets the factory method that creates instances of <see cref="ResilienceStrategyBuilder"/>.
    /// </summary>
    [Required]
    public Func<ResilienceStrategyBuilder> BuilderFactory { get; set; } = () => new ResilienceStrategyBuilder();

    /// <summary>
    /// Gets or sets the comparer that is used by the registry to retrieve the resilience strategies.
    /// </summary>
    [Required]
    public IEqualityComparer<TKey> StrategyComparer { get; set; } = EqualityComparer<TKey>.Default;

    /// <summary>
    /// Gets or sets the comparer that is used by the registry to retrieve the resilience strategy builders.
    /// </summary>
    [Required]
    public IEqualityComparer<TKey> BuilderComparer { get; set; } = EqualityComparer<TKey>.Default;
}
