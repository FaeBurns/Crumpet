namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class NonTerminalConstraint : NodeConstraint
{
    public Type NonTerminalType { get; }

    protected NonTerminalConstraint(Type nonTerminalType) : base(true)
    {
        NonTerminalType = nonTerminalType;
    }

    public override string ToString()
    {
        return NonTerminalType.Name.Replace("Node", String.Empty);
    }
}

public class NonTerminalConstraint<T> : NonTerminalConstraint
{
    public NonTerminalConstraint() : base(typeof(T)) { }
}