﻿using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

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
        yield return new NonTerminalDefinition<UnaryExpressionNode>(
            new NonTerminalConstraint<ExpressionWithPostfixNode>(),
            GetNodeConstructor<UnaryExpressionNode>());
    }
}