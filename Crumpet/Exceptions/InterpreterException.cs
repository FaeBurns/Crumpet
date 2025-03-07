using Parser;
using Shared;

namespace Crumpet.Exceptions;

public class InterpreterException : Exception
{
    public SourceLocation SourceLocation { get; }

    public InterpreterException(SourceLocation? sourceLocation, string message) : base(message)
    {
        SourceLocation = sourceLocation ?? new SourceLocation();
    }

    public InterpreterException(SourceLocation? sourceLocation, string message, Exception inner) : base(message, inner)
    {
        SourceLocation = sourceLocation ?? new SourceLocation();
    }
}