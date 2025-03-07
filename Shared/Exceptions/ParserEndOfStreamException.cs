namespace Shared.Exceptions;

public class ParserEndOfStreamException : Exception
{
    public ParserEndOfStreamException(string message) : base(message)
    {
    }
}