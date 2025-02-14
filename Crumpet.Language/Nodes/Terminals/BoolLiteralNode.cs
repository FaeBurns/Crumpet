using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class BoolLiteralNode : TerminalNode, ITerminalNodeFactory
{
    public bool BoolLiteral { get; }
    
    public BoolLiteralNode(string terminal) : base(terminal)
    {
        BoolLiteral = Convert.ToBoolean(terminal);
    }

    public static IEnumerable<TerminalDefinition> GetTerminals()
    {
        yield return new TerminalDefinition("boolLiteral", "true|false", GetNodeConstructor<BoolLiteralNode>());
    }
}