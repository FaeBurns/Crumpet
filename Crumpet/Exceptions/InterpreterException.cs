using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Parser;
using Shared;

namespace Crumpet.Exceptions;

public class InterpreterException : Exception
{
    public SourceLocation SourceLocation { get; }

    public InterpreterException(SourceLocation sourceLocation, string message, Exception inner) : base(ConstructMessage(sourceLocation, message, inner), inner)
    {
        SourceLocation = sourceLocation;
    }

    private static string ConstructMessage(SourceLocation? location, string message, Exception inner)
    {
        return (location ?? new SourceLocation()) + Environment.NewLine + message + Environment.NewLine + inner.Message.Split(Environment.NewLine).First();
    }
}