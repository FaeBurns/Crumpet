using Crumpet.Language.Nodes.Constraints;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class AndExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExclusiveOrExpressionNode Primary { get; }
    public ExclusiveOrExpressionNode? Secondary { get; }

    public AndExpressionNode(ExclusiveOrExpressionNode primary, ExclusiveOrExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<AndExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ExclusiveOrExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.AND_AND),
                        new NonTerminalConstraint<ExclusiveOrExpressionNode>()))),
            GetNodeConstructor<AndExpressionNode>());
    }
}