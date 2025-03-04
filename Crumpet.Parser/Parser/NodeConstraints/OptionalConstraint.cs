using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OptionalConstraint : ContainsSingleConstraint
{
    public OptionalConstraint(NodeConstraint constraint) : base(constraint)
    {
    }

    public override string ToString()
    {
        return "(" + Constraint.ToString() + ")?";
    }

    public override ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        ParserElement? childResult = Constraint.WalkStream(stream, registry);

        if (childResult is not null)
            return childResult;

        return new NullParserElement();
    }
}