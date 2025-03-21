using System.Collections;
using Crumpet.Instructions.Unary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class UnaryExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TerminalNode<CrumpetToken>? Variant { get; }
    public ExpressionWithPostfixNode Expression { get; }

    public UnaryExpressionNode(TerminalNode<CrumpetToken>? variant, ExpressionWithPostfixNode expression) : base(expression)
    {
        Variant = variant;
        Expression = expression;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<UnaryExpressionNode>(
            new SequenceConstraint(
                new OptionalConstraint(new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.MINUS),
                    new CrumpetTerminalConstraint(CrumpetToken.NOT))),
                new NonTerminalConstraint<ExpressionWithPostfixNode>()),
            GetNodeConstructor<UnaryExpressionNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;

        if (Variant is null)
            yield break;

        if (Variant.Token.TokenId == CrumpetToken.MINUS)
            yield return new NegativeNumberInstruction(Location);
        else if (Variant.Token.TokenId == CrumpetToken.NOT)
            yield return new LogicalNotInstruction(Location);
    }
}