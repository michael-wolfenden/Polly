using Polly.Simmy;

namespace Polly.Core.Tests.Simmy.Outcomes;

public static class EnabledGeneratorArgumentsTests
{
    [Fact]
    public static void Ctor_Ok()
    {
        // Arrange
        var context = ResilienceContextPool.Shared.Get(TestContext.Current.CancellationToken);

        // Act
        var args = new EnabledGeneratorArguments(context);

        // Assert
        args.Context.Should().Be(context);
    }
}
