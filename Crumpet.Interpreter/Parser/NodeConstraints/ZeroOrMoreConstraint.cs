namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class ZeroOrMoreConstraint : MultiNodeConstraint
{
    public ZeroOrMoreConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints, true)
    {
    }
}