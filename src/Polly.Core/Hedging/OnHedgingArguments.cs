namespace Polly.Hedging;

/// <summary>
/// Represents arguments used by the on-hedging event.
/// </summary>
/// <param name="Context">The context associated with the execution of a user-provided callback.</param>
/// <param name="Attempt">The zero-based hedging attempt number.</param>
/// <remarks>
/// Always use the constructor when creating this struct, otherwise we do not guarantee binary compatibility.
/// </remarks>
public record OnHedgingArguments(ResilienceContext Context, int Attempt);
