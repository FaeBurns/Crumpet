namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class ZeroOrMoreConstraint : ContainsSingleConstraint
{
    public ZeroOrMoreConstraint(NodeConstraint constraint) : base(constraint, true)
    {
    }

    public override string ToString()
    {
        return Constraint.ToString() + "*"; 
    }
}