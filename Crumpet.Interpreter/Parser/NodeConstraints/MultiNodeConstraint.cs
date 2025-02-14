namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class MultiNodeConstraint : NodeConstraint
{
    public IEnumerable<NodeConstraint> Constraints { get; }

    protected MultiNodeConstraint(IEnumerable<NodeConstraint> constraints, bool includeInConstructor) : base(includeInConstructor)
    {
        Constraints = constraints;
    }
}