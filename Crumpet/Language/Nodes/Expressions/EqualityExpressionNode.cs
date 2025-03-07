using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class EqualityExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public RelationExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public RelationExpressionNode? Secondary { get; }

    public EqualityExpressionNode(RelationExpressionNode primary, TerminalNode<CrumpetToken> sugar, RelationExpressionNode? secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public EqualityExpressionNode(RelationExpressionNode primary)
    {
        Primary = primary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<EqualityExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<RelationExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.EQUALS_EQUALS),
                        new CrumpetTerminalConstraint(CrumpetToken.NOT_EQUALS)),
                    new NonTerminalConstraint<RelationExpressionNode>())),
            GetNodeConstructor<EqualityExpressionNode>(3));

        yield return new NonTerminalDefinition<EqualityExpressionNode>(
            new NonTerminalConstraint<RelationExpressionNode>(),
            GetNodeConstructor<EqualityExpressionNode>(1));
    }
}