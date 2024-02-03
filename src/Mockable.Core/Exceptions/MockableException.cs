namespace Mockable.Core.Exceptions;

public class MockableException : Exception
{
    public MockableException()
    {
    }

    public MockableException(string? message)
        :base(message)
    {
    }

    public MockableException(string? message, Exception? innerException)
        :base(message, innerException)
    {
    }
}
