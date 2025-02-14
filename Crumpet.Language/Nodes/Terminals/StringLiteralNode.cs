using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class StringLiteralNode : TerminalNode, ITerminalNodeFactory
{
    public StringLiteralNode(string terminal) : base(terminal)
    {
    }

    public static IEnumerable<TerminalDefinition> GetTerminals()
    {
        // \".*\"
        yield return new TerminalDefinition("stringLiteral", "\\\".*\\\"", GetNodeConstructor<StringLiteralNode>());
    }
}