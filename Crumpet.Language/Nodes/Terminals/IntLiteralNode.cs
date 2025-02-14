using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IntLiteralNode : TerminalNode, ITerminalNodeFactory
{
    public int IntLiteral { get; }
    
    public IntLiteralNode(string terminal) : base(terminal)
    {
        IntLiteral = Convert.ToInt32(terminal);
    }

    public static IEnumerable<TerminalDefinition> GetTerminals()
    {
        yield return new TerminalDefinition("intLiteral", "-?[0-9]+", GetNodeConstructor<IntLiteralNode>());
    }
}