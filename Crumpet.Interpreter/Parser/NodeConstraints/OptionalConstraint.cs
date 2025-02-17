namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OptionalConstraint : ContainsSingleConstraint
{

    public OptionalConstraint(NodeConstraint constraint) : base(constraint, true)
    {
    }

    public override string ToString()
    {
        return Constraint.ToString() + "?";
    }
}