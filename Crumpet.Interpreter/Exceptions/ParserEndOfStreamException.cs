namespace Crumpet.Interpreter.Exceptions;

public class ParserEndOfStreamException : Exception
{
    public ParserEndOfStreamException(string message) : base(message)
    {
    }
}