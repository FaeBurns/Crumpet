using System.Diagnostics;
using Crumpet.Instructions.Unary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ExpressionWithPointerPrefixNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public PrimaryExpressionNode Expression { get; }

    public ExpressionWithPointerPrefixNode(TerminalNode<CrumpetToken>? sugar, PrimaryExpressionNode expression) : base(sugar, expression)
    {
        Sugar = sugar;
        Expression = expression;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExpressionWithPointerPrefixNode>(
            new SequenceConstraint(
                new OptionalConstraint(new OrConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY), new CrumpetTerminalConstraint(CrumpetToken.REFERENCE))),
                new NonTerminalConstraint<PrimaryExpressionNode>()),
            GetNodeConstructor<ExpressionWithPointerPrefixNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;

        if (Sugar == null)
            yield break;

        yield return Sugar.Token.TokenId switch
        {
            CrumpetToken.MULTIPLY => new DereferencePointerInstruction(Location),
            CrumpetToken.REFERENCE => new CreateReferenceToInstruction(Location),
            _ => throw new UnreachableException(),
        };
    }
}