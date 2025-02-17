namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class TerminalConstraint<T> : NodeConstraint where T : Enum
{
    public T Token { get; }

    public TerminalConstraint(T token, bool includeInConstructor = true) : base(includeInConstructor)
    {
        Token = token;
    }

    public override string ToString()
    {
        return Token.ToString();
    }
}