using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

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
        yield return new NonTerminalDefinition("argumentExpressionList",
            new SequenceConstraint(
                new NonTerminalConstraint("expression"),
                new ZeroOrMoreConstraint(
                    new SequenceConstraint(
                        new RawTerminalConstraint(","),
                        new NonTerminalConstraint("expression")))),
            GetNodeConstructor<ArgumentExpressionListNode>());
    }
}