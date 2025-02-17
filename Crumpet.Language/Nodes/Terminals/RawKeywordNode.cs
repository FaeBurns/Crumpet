using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class RawKeywordNode : TerminalNode<CrumpetToken>
{
    public RawKeywordNode(string terminal) : base(terminal)
    {
    }
}