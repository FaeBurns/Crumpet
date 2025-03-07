using Parser.NodeConstraints;

namespace Crumpet.Language.Nodes.Constraints;

public class CrumpetTerminalConstraint : TerminalConstraint<CrumpetToken>
{
    public CrumpetTerminalConstraint(CrumpetToken token, bool includeInConstructor = true) : base(token, includeInConstructor)
    {
    }
}