namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class NonTerminalConstraint : NodeConstraint
{
    public string NonTerminalName { get; }

    public NonTerminalConstraint(string nonTerminalName) : base(true)
    {
        NonTerminalName = nonTerminalName;
    }
}