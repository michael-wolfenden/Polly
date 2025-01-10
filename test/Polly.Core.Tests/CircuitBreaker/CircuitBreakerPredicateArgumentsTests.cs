using Polly.CircuitBreaker;

namespace Polly.Core.Tests.CircuitBreaker;

public static class CircuitBreakerPredicateArgumentsTests
{
    [Fact]
    public static void Ctor_Ok()
    {
        var args = new CircuitBreakerPredicateArguments<int>(
            ResilienceContextPool.Shared.Get(TestContext.Current.CancellationToken),
            Outcome.FromResult(1));

        args.Context.Should().NotBeNull();
        args.Outcome.Result.Should().Be(1);
    }
}
