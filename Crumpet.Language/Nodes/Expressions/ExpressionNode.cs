using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public AssignmentExpressionNode Expression { get; }

    public ExpressionNode(AssignmentExpressionNode expression)
    {
        Expression = expression;
    }
    
    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        yield return Expression;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExpressionNode>(
            new NonTerminalConstraint<AssignmentExpressionNode>(),
            GetNodeConstructor<ExpressionNode>());
    }
}