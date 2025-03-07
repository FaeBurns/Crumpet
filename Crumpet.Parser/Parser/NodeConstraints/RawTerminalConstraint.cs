namespace Crumpet.Parser.NodeConstraints;

public class RawTerminalConstraint<T> : TerminalConstraint<T> where T : Enum
{
    public RawTerminalConstraint(T token) : base(token, false)
    {
    }
}