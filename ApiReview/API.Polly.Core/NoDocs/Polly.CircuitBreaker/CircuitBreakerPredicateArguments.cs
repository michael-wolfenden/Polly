// Assembly 'Polly.Core'

using System.Runtime.InteropServices;

namespace Polly.CircuitBreaker;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct CircuitBreakerPredicateArguments
{
    public CircuitBreakerPredicateArguments();
}
