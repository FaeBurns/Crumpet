namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OneOrMoreConstraint : MultiNodeConstraint
{
    public OneOrMoreConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints, true)
    {
    }
}