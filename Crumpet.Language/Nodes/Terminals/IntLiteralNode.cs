using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IntLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public int IntLiteral { get; }
    
    public IntLiteralNode(string terminal) : base(terminal)
    {
        IntLiteral = Convert.ToInt32(terminal);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.INT, GetNodeConstructor<IntLiteralNode>());
    }
}