namespace Parser.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(int lineNumber, int columnNumber) : base($"Invalid token found at line {lineNumber} column {columnNumber}")
    {
    }
}