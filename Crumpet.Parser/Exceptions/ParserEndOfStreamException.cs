namespace Crumpet.Parser.Exceptions;

public class ParserEndOfStreamException : Exception
{
    public ParserEndOfStreamException(string message) : base(message)
    {
    }
}