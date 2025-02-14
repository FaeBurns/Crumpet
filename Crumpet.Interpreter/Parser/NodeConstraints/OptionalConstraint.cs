namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class OptionalConstraint : NodeConstraint
{
    public NodeConstraint Inner { get; }

    public OptionalConstraint(NodeConstraint inner) : base(true)
    {
        Inner = inner;
    }
}