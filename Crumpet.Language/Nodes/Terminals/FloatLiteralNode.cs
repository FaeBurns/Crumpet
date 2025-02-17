using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class FloatLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public float FloatLiteral { get; }
    
    public FloatLiteralNode(string terminal) : base(terminal)
    {
        FloatLiteral = Convert.ToSingle(terminal);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.FLOAT, GetNodeConstructor<FloatLiteralNode>());
    }
}