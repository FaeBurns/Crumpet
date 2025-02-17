namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class ContainsSingleConstraint : NodeConstraint
{
    public NodeConstraint Constraint { get; }

    protected ContainsSingleConstraint(NodeConstraint constraint, bool includeInConstructor) : base(includeInConstructor)
    {
        Constraint = constraint;
    }
}