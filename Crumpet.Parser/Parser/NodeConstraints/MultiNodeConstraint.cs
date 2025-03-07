namespace Crumpet.Parser.NodeConstraints;

public abstract class MultiNodeConstraint : NodeConstraint
{
    public IEnumerable<NodeConstraint> Constraints { get; }

    protected MultiNodeConstraint(IEnumerable<NodeConstraint> constraints)
    {
        Constraints = constraints;
    }
}