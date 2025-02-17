namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OneOrMoreConstraint : ContainsSingleConstraint
{
    public OneOrMoreConstraint(NodeConstraint constraint) : base(constraint, true)
    {
    }

    public override string ToString()
    {
        return Constraint.ToString() + "+";
    }
}