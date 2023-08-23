// Assembly 'Polly.Core'

using System;

namespace Polly.CircuitBreaker;

public sealed class CircuitBreakerStateProvider
{
    public CircuitState CircuitState { get; }
    public CircuitBreakerStateProvider();
}
