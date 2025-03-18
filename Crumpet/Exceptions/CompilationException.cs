using Parser.Nodes;

namespace Crumpet.Exceptions;

public class CompilationException : Exception
{
    public NonTerminalNode Node { get; }

    public CompilationException(NonTerminalNode node, string message) : base(message)
    {
        Node = node;
    }
}