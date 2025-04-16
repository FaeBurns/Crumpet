namespace Crumpet.Exceptions;

public class RuntimeAssertationException : Exception
{
    public RuntimeAssertationException(string message) : base(message)
    {
    }

    public RuntimeAssertationException(ReadOnlySpan<char> message) : base(message.ToString())
    {

    }
}