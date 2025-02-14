namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OrConstraint : MultiNodeConstraint
{
    public OrConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints, true)
    {
    }
}