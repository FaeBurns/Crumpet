using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IdentifierNode : TerminalNode, ITerminalNodeFactory
{
    public IdentifierNode(string terminal) : base(terminal)
    {
    }
    
    public static IEnumerable<TerminalDefinition> GetTerminals()
    {
        yield return new TerminalDefinition("identifier", "[a-zA-Z_]+[a-zA-Z0-9_]*", GetNodeConstructor<IdentifierNode>());
    }
}