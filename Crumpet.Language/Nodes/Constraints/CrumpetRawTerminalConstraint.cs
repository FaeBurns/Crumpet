using Crumpet.Interpreter.Parser.NodeConstraints;

namespace Crumpet.Language.Nodes.Constraints;

public class CrumpetRawTerminalConstraint : RawTerminalConstraint<CrumpetToken>
{
    public CrumpetRawTerminalConstraint(CrumpetToken token) : base(token)
    {
    }
}

public class CrumpetTerminalConstraint : TerminalConstraint<CrumpetToken>
{
    public CrumpetTerminalConstraint(CrumpetToken token, bool includeInConstructor = true) : base(token, includeInConstructor)
    {
    }
}