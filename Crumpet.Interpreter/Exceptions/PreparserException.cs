using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Exceptions;

public class PreparserException : Exception
{
    public PreparserException(string message, NonTerminalNode node)
    {
    }
}