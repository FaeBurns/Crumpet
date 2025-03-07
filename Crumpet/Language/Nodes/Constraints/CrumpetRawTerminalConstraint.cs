using Parser.NodeConstraints;

namespace Crumpet.Language.Nodes.Constraints;

public class CrumpetRawTerminalConstraint : RawTerminalConstraint<CrumpetToken>
{
    public CrumpetRawTerminalConstraint(CrumpetToken token) : base(token)
    {
    }
}