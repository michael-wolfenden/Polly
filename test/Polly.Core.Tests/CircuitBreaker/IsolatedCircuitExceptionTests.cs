using Polly.CircuitBreaker;

namespace Polly.Core.Tests.CircuitBreaker;

public class IsolatedCircuitExceptionTests
{
    [Fact]
    public void Ctor_Default_Ok()
    {
        var exception = new IsolatedCircuitException();
        exception.Message.Should().Be("The circuit is manually held open and is not allowing calls.");
        exception.RetryAfter.Should().BeNull();
    }

    [Fact]
    public void Ctor_Message_Ok()
    {
        var exception = new IsolatedCircuitException(TestMessage);
        exception.Message.Should().Be(TestMessage);
        exception.RetryAfter.Should().BeNull();
    }

    [Fact]
    public void Ctor_Message_InnerException_Ok()
    {
        var exception = new IsolatedCircuitException(TestMessage, new InvalidOperationException());
        exception.Message.Should().Be(TestMessage);
        exception.InnerException.Should().BeOfType<InvalidOperationException>();
        exception.RetryAfter.Should().BeNull();
    }

#if !NETCOREAPP
    [Fact]
    public void BinarySerialization_Ok() =>
        BinarySerializationUtil.SerializeAndDeserializeException(new IsolatedCircuitException("dummy")).Should().NotBeNull();
#endif

    private const string TestMessage = "Dummy.";
}
