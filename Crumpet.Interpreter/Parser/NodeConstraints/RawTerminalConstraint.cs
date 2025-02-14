namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class RawTerminalConstraint : NodeConstraint
{
    public string Terminal { get; }

    public RawTerminalConstraint(string terminal, bool includeInConstructor = false) : base(includeInConstructor)
    {
        Terminal = terminal;
    }
}