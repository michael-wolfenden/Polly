using System.ComponentModel.DataAnnotations;

namespace Polly.Hedging;

/// <summary>
/// Hedging strategy options.
/// </summary>
/// <typeparam name="TResult">The type of result these hedging options handle.</typeparam>
public class HedgingStrategyOptions<TResult> : ResilienceStrategyOptions
{
    /// <summary>
    /// Gets or sets the minimal time of waiting before spawning a new hedged call.
    /// </summary>
    /// <remarks>
    /// You can also use <see cref="TimeSpan.Zero"/> to create all hedged tasks (value of <see cref="MaxHedgedAttempts"/>) at once
    /// or <see cref="System.Threading.Timeout.InfiniteTimeSpan"/> to force the hedging strategy to never create new task before the old one is finished.
    /// <para> If you want a greater control over hedging delay customization use <see cref="HedgingDelayGenerator"/>.</para>
    /// </remarks>
    /// <value>
    /// The default value is 2 seconds.
    /// </value>
    public TimeSpan HedgingDelay { get; set; } = HedgingConstants.DefaultHedgingDelay;

    /// <summary>
    /// Gets or sets the maximum hedged attempts to perform the desired task.
    /// </summary>
    /// <remarks>
    /// The value defines how many concurrent hedged tasks will be triggered by the strategy.
    /// This includes the primary hedged task that is initially performed, and the further tasks that will
    /// be fetched from the provider and spawned in parallel.
    /// </remarks>
    /// <value>
    /// The default value is 2. The value must be bigger or equal to 2, and lower or equal to 10.
    /// </value>
    [Range(HedgingConstants.MinimumHedgedAttempts, HedgingConstants.MaximumHedgedAttempts)]
    public int MaxHedgedAttempts { get; set; } = HedgingConstants.DefaultMaxHedgedAttempts;

    /// <summary>
    /// Gets or sets the predicate that determines whether a hedging should be performed for a given result.
    /// </summary>
    /// <value>
    /// The default value is a predicate that hedges on any exception except <see cref="OperationCanceledException"/>.
    /// This property is required.
    /// </value>
    [Required]
    public Func<OutcomeArguments<TResult, HedgingPredicateArguments>, ValueTask<bool>> ShouldHandle { get; set; } = DefaultPredicates<HedgingPredicateArguments, TResult>.HandleOutcome;

    /// <summary>
    /// Gets or sets the hedging action generator that creates hedged actions.
    /// </summary>
    /// <value>
    /// The default generator executes the original callback that was passed to the hedging resilience strategy. This property is required.
    /// </value>
    [Required]
    public Func<HedgingActionGeneratorArguments<TResult>, Func<ValueTask<Outcome<TResult>>>?> HedgingActionGenerator { get; set; } = args =>
    {
        return async () =>
        {
            if (args.PrimaryContext.IsSynchronous)
            {
                return await Task.Run(() => args.Callback(args.ActionContext).AsTask()).ConfigureAwait(args.ActionContext.ContinueOnCapturedContext);
            }

            return await args.Callback(args.ActionContext).ConfigureAwait(args.ActionContext.ContinueOnCapturedContext);
        };
    };

    /// <summary>
    /// Gets or sets the generator that generates hedging delays for each hedging attempt.
    /// </summary>
    /// <remarks>
    /// The <see cref="HedgingDelayGenerator"/> takes precedence over <see cref="HedgingDelay"/>. If specified, the <see cref="HedgingDelay"/> is ignored.
    /// </remarks>
    /// <value>
    /// The default value is <see langword="null"/>.
    /// </value>
    public Func<HedgingDelayArguments, ValueTask<TimeSpan>>? HedgingDelayGenerator { get; set; }

    /// <summary>
    /// Gets or sets the event that is raised when a hedging is performed.
    /// </summary>
    /// <remarks>
    /// The hedging is executed when the current attempt outcome is not successful and the <see cref="ShouldHandle"/> predicate returns <see langword="true"/> or when
    /// the current attempt did not finish within the <see cref="HedgingDelay"/>.
    /// </remarks>
    /// <value>
    /// The default value is <see langword="null"/>.
    /// </value>
    public Func<OutcomeArguments<TResult, OnHedgingArguments>, ValueTask>? OnHedging { get; set; }
}
