using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IdentifierNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public IdentifierNode(string terminal) : base(terminal)
    {
    }
    
    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.IDENTIFIER, GetNodeConstructor<IdentifierNode>());
    }
}