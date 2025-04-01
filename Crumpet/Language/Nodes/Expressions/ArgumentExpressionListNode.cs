using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ArgumentExpressionListNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public NonTerminalNode[] Expressions { get; }

    public ArgumentExpressionListNode(NonTerminalNode firstExpression, IEnumerable<NonTerminalNode> otherExpressions)
    {
        Expressions = otherExpressions.Prepend(firstExpression).ToArray();
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (NonTerminalNode node in Expressions)
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

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expressions;
    }
}