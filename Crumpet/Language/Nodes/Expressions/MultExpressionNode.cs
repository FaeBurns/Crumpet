using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class MultExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public UnaryExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public UnaryExpressionNode? Secondary { get; }

    public MultExpressionNode(UnaryExpressionNode primary, TerminalNode<CrumpetToken> sugar, UnaryExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public MultExpressionNode(UnaryExpressionNode primary)
    {
        Primary = primary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<MultExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<UnaryExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY),
                        new CrumpetTerminalConstraint(CrumpetToken.DIVIDE)),
                    new NonTerminalConstraint<UnaryExpressionNode>())),
            GetNodeConstructor<MultExpressionNode>(3));

        yield return new NonTerminalDefinition<MultExpressionNode>(
            new NonTerminalConstraint<UnaryExpressionNode>(),
            GetNodeConstructor<MultExpressionNode>(1));
    }
}