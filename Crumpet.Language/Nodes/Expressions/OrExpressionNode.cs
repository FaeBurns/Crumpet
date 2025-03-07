using Crumpet.Language.Nodes.Constraints;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class OrExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public AndExpressionNode Primary { get; }
    public AndExpressionNode? Secondary { get; }

    public OrExpressionNode(AndExpressionNode primary, AndExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<OrExpressionNode>( 
            new SequenceConstraint(
                new NonTerminalConstraint<AndExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.OR),
                        new NonTerminalConstraint<AndExpressionNode>()))),
            GetNodeConstructor<OrExpressionNode>());
    }
}