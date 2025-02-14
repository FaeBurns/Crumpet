using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class UnaryExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExpressionWithPostfixNode Expression { get; }

    public UnaryExpressionNode(ExpressionWithPostfixNode expression) : base(expression)
    {
        Expression = expression;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("unaryExpression",
            new NonTerminalConstraint("expressionWithPostfix"),
            GetNodeConstructor<UnaryExpressionNode>());
    }
}