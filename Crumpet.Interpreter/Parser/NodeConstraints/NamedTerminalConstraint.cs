namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class NamedTerminalConstraint : NodeConstraint
{
    public string Name { get; }

    public NamedTerminalConstraint(string name) : base(true)
    {
        Name = name;
    }
}