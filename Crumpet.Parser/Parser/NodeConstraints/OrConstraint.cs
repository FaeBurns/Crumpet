using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OrConstraint : MultiNodeConstraint
{
    public OrConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints)
    {
    }

    public override string ToString()
    {
        return string.Join(" | ", Constraints.Select(c => c.ToString()));
    }

    public override ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        int position = stream.Position;
        foreach (NodeConstraint inner in Constraints)
        {
            // only one node needs to match
            ParserElement? collectionNode = inner.WalkStream(stream, registry);
            if (collectionNode is not null)
                return collectionNode;

            // reset position after each node
            stream.Position = position;
        }

        return null;
    }
}