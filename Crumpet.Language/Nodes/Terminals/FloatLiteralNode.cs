using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class FloatLiteralNode : TerminalNode, ITerminalNodeFactory
{
    public float FloatLiteral { get; }
    
    public FloatLiteralNode(string terminal) : base(terminal)
    {
        FloatLiteral = Convert.ToSingle(terminal);
    }

    public static IEnumerable<TerminalDefinition> GetTerminals()
    {
        yield return new TerminalDefinition("floatLiteral", "-?[0-9]+\\.[0-9]+", GetNodeConstructor<FloatLiteralNode>());
    }
}