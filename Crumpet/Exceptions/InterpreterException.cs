using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Parser;
using Shared;

namespace Crumpet.Exceptions;

public class InterpreterException : Exception
{
    public SourceLocation SourceLocation { get; }

    public InterpreterException(InterpreterExecutionContext context, string message) : base(message)
    {
        SourceLocation = context.CurrentUnit?.UnitLocation ?? new SourceLocation();
    }

    public InterpreterException(SourceLocation? sourceLocation, string message) : base(message)
    {
        SourceLocation = sourceLocation ?? new SourceLocation();
    }

    public InterpreterException(SourceLocation sourceLocation, string message, Exception inner) : base(ConstructMessage(sourceLocation, message), inner)
    {
        SourceLocation = sourceLocation;
    }

    private static string ConstructMessage(SourceLocation location, string message)
    {
        return (location ?? new SourceLocation()) + Environment.NewLine + message;
    }
}