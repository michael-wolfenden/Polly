namespace Polly.CircuitBreaker;

/// <summary>
/// Allows to retrieve the circuit breaker state.
/// </summary>
public sealed class CircuitBreakerStateProvider
{
    private Func<CircuitState>? _circuitStateProvider;
    private Func<Exception?>? _lastExceptionProvider;

    internal void Initialize(Func<CircuitState> circuitStateProvider, Func<Exception?> lastExceptionProvider)
    {
        if (_circuitStateProvider != null)
        {
            throw new InvalidOperationException($"This instance of '{nameof(CircuitBreakerStateProvider)}' is already initialized and cannot be used in a different circuit-breaker strategy.");
        }

        _circuitStateProvider = circuitStateProvider;
        _lastExceptionProvider = lastExceptionProvider;
    }

    /// <summary>
    /// Gets a value indicating whether the state provider is initialized.
    /// </summary>
    /// <remarks>
    /// The initialization happens when the circuit-breaker strategy is attached to this class.
    /// This happens when the final strategy is created by the <see cref="ResilienceStrategyBuilder.Build"/> call.
    /// </remarks>
    public bool IsInitialized => _circuitStateProvider != null;

    /// <summary>
    /// Gets the state of the underlying circuit.
    /// </summary>
    public CircuitState CircuitState => _circuitStateProvider?.Invoke() ?? CircuitState.Closed;

    /// <summary>
    /// Gets the last exception handled by the circuit-breaker.
    /// <remarks>This will be null if no exceptions have been handled by the circuit-breaker since the circuit last closed.</remarks>
    /// </summary>
    public Exception? LastException => _lastExceptionProvider?.Invoke() ?? null;
}
