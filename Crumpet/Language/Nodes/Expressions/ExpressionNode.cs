using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
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

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
    }
}