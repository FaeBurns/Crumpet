using Parser.Nodes;

namespace Crumpet.Exceptions;

public class PreparserException : Exception
{
    public PreparserException(string message, NonTerminalNode node)
    {
    }
}