namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class SequenceConstraint : MultiNodeConstraint
{
    public SequenceConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints, true)
    {
    }
}