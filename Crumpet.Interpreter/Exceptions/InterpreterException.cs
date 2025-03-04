using Crumpet.Interpreter.Parser;

namespace Crumpet.Interpreter.Exceptions;

public class InterpreterException : Exception
{
    public SourceLocation SourceLocation { get; }

    public InterpreterException(SourceLocation sourceLocation, string message) : base(message)
    {
        SourceLocation = sourceLocation;
    }

    public InterpreterException(SourceLocation sourceLocation, string message, Exception inner) : base(message, inner)
    {
        SourceLocation = sourceLocation;
    }
}