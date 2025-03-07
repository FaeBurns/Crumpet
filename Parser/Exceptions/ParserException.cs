namespace Parser.Exceptions;

public class ParserException : Exception
{
    public SourceLocation Location { get; }

    public ParserException(string message, SourceLocation location) : base(message)
    {
        Location = location;
    }

    public ParserException(string message, SourceLocation location, Exception innerException) : base(message, innerException)
    {
        Location = location;
    }
}