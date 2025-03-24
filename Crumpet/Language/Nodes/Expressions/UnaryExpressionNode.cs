using System.Collections;
using System.Diagnostics;
using Crumpet.Instructions;
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
        // this should really contain an ExpressionWithPrefixNode and handle all that in there but this one doesn't really do anything by itself?
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

        yield return Variant.Token.TokenId switch
        {
            CrumpetToken.MINUS => new NegativeNumberInstruction(Location),
            CrumpetToken.NOT => new LogicalNotInstruction(Location),
            _ => throw new UnreachableException(),
        };
    }
}