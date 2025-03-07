using Crumpet.Parser.Elements;
using Crumpet.Parser.Nodes;

namespace Crumpet.Parser.NodeConstraints;

public abstract class NodeConstraint
{
    public abstract override string ToString();

    public abstract ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry) where T : Enum;
}