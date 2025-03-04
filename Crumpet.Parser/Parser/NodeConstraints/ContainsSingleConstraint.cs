namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class ContainsSingleConstraint : NodeConstraint
{
    public NodeConstraint Constraint { get; }

    protected ContainsSingleConstraint(NodeConstraint constraint)
    {
        Constraint = constraint;
    }
}