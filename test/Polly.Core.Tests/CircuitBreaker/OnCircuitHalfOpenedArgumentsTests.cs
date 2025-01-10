using Polly.CircuitBreaker;

namespace Polly.Core.Tests.CircuitBreaker;

public static class OnCircuitHalfOpenedArgumentsTests
{
    [Fact]
    public static void Ctor_Ok()
    {
        // Arrange
        var context = ResilienceContextPool.Shared.Get(TestContext.Current.CancellationToken);

        // Act
        var target = new OnCircuitHalfOpenedArguments(context);

        // Assert
        target.Context.Should().Be(context);
    }
}
