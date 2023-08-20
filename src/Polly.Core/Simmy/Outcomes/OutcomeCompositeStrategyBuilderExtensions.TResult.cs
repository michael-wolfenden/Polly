﻿using System.Diagnostics.CodeAnalysis;

namespace Polly.Simmy.Outcomes;

/// <summary>
/// Extension methods for adding outcome to a <see cref="CompositeStrategyBuilder"/>.
/// </summary>
public static partial class OutcomeCompositeStrategyBuilderExtensions
{
    /// <summary>
    /// Adds a fault chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="enabled">A value that indicates whether or not the chaos strategy is enabled for a given execution.</param>
    /// <param name="injectionRate">The injection rate for a given execution, which the value should be between [0, 1].</param>
    /// <param name="fault">The exception to inject.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddFault<TResult>(this CompositeStrategyBuilder<TResult> builder, bool enabled, double injectionRate, Exception fault)
    {
        Guard.NotNull(builder);

        builder.AddFaultCore<TResult>(new OutcomeStrategyOptions<Exception>
        {
            Enabled = enabled,
            InjectionRate = injectionRate,
            Outcome = new(fault)
        });
        return builder;
    }

    /// <summary>
    /// Adds a fault chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="enabled">A value that indicates whether or not the chaos strategy is enabled for a given execution.</param>
    /// <param name="injectionRate">The injection rate for a given execution, which the value should be between [0, 1].</param>
    /// <param name="faultGenerator">The exception generator delegate.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddFault<TResult>(
        this CompositeStrategyBuilder<TResult> builder, bool enabled, double injectionRate, Func<ValueTask<Outcome<Exception>?>> faultGenerator)
    {
        Guard.NotNull(builder);

        builder.AddFaultCore<TResult>(new OutcomeStrategyOptions<Exception>
        {
            Enabled = enabled,
            InjectionRate = injectionRate,
            OutcomeGenerator = (_) => faultGenerator()
        });
        return builder;
    }

    /// <summary>
    /// Adds a fault chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="options">The fault strategy options.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddFault<TResult>(this CompositeStrategyBuilder<TResult> builder, OutcomeStrategyOptions<Exception> options)
    {
        Guard.NotNull(builder);
        Guard.NotNull(options);

        builder.AddFaultCore<TResult>(options);
        return builder;
    }

    /// <summary>
    /// Adds an outcome chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="enabled">A value that indicates whether or not the chaos strategy is enabled for a given execution.</param>
    /// <param name="injectionRate">The injection rate for a given execution, which the value should be between [0, 1].</param>
    /// <param name="result">The outcome to inject.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddResult<TResult>(this CompositeStrategyBuilder<TResult> builder, bool enabled, double injectionRate, TResult result)
    {
        Guard.NotNull(builder);

        builder.AddOutcomeCore<TResult, OutcomeStrategyOptions<TResult>>(new OutcomeStrategyOptions<TResult>
        {
            Enabled = enabled,
            InjectionRate = injectionRate,
            Outcome = new(result)
        });
        return builder;
    }

    /// <summary>
    /// Adds an outcome chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="enabled">A value that indicates whether or not the chaos strategy is enabled for a given execution.</param>
    /// <param name="injectionRate">The injection rate for a given execution, which the value should be between [0, 1].</param>
    /// <param name="outcomeGenerator">The outcome generator delegate.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddResult<TResult>(
        this CompositeStrategyBuilder<TResult> builder, bool enabled, double injectionRate, Func<ValueTask<Outcome<TResult>?>> outcomeGenerator)
    {
        Guard.NotNull(builder);

        builder.AddOutcomeCore<TResult, OutcomeStrategyOptions<TResult>>(new OutcomeStrategyOptions<TResult>
        {
            Enabled = enabled,
            InjectionRate = injectionRate,
            OutcomeGenerator = (_) => outcomeGenerator()
        });
        return builder;
    }

    /// <summary>
    /// Adds an outcome chaos strategy to the builder.
    /// </summary>
    /// <typeparam name="TResult">The type of result the retry strategy handles.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="options">The outcome strategy options.</param>
    /// <returns>The builder instance with the retry strategy added.</returns>
    public static CompositeStrategyBuilder<TResult> AddResult<TResult>(this CompositeStrategyBuilder<TResult> builder, OutcomeStrategyOptions<TResult> options)
    {
        Guard.NotNull(builder);
        Guard.NotNull(options);

        builder.AddOutcomeCore<TResult, OutcomeStrategyOptions<TResult>>(options);
        return builder;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "All options members preserved.")]
    private static void AddOutcomeCore<TResult, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(
        this CompositeStrategyBuilder<TResult> builder,
        OutcomeStrategyOptions<TResult> options)
    {
        builder.AddStrategy(
            context => new OutcomeChaosStrategy<TResult>(options, context.Telemetry),
            options);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
        Justification = "All options members preserved.")]
    private static void AddFaultCore<TResult>(
        this CompositeStrategyBuilder<TResult> builder,
        OutcomeStrategyOptions<Exception> options)
    {
        builder.AddStrategy(
            context => new OutcomeChaosStrategy<TResult>(options, context.Telemetry),
            options);
    }
}
