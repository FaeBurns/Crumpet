using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ArgumentExpressionListNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExpressionNode[] Expressions { get; }

    public ArgumentExpressionListNode(ExpressionNode firstExpression, IEnumerable<ExpressionNode> otherExpressions)
    {
        Expressions = otherExpressions.Prepend(firstExpression).ToArray();
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (ExpressionNode node in Expressions)
        {
            yield return node;
        }
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ArgumentExpressionListNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ExpressionNode>(),
                new ZeroOrMoreConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.COMMA),
                        new NonTerminalConstraint<ExpressionNode>()))),
            GetNodeConstructor<ArgumentExpressionListNode>());
    }
}