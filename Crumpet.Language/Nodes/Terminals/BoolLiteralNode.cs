using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class BoolLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public bool BoolLiteral { get; }
    
    public BoolLiteralNode(string terminal) : base(terminal)
    {
        BoolLiteral = Convert.ToBoolean(terminal);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.BOOL, GetNodeConstructor<BoolLiteralNode>());
    }
}