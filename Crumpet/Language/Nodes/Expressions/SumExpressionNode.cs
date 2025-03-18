using Crumpet.Instructions;
using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class SumExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public MultExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public MultExpressionNode? Secondary { get; }

    public SumExpressionNode(MultExpressionNode primary, TerminalNode<CrumpetToken> sugar, MultExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public SumExpressionNode(MultExpressionNode primary) : base(primary)
    {
        Primary = primary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // sum
        yield return new NonTerminalDefinition<SumExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<MultExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.PLUS),
                        new CrumpetTerminalConstraint(CrumpetToken.MINUS)),
                    new NonTerminalConstraint<MultExpressionNode>())),
            GetNodeConstructor<SumExpressionNode>(3));

        // passthrough
        yield return new NonTerminalDefinition<SumExpressionNode>(
                new NonTerminalConstraint<MultExpressionNode>(),
            GetNodeConstructor<SumExpressionNode>(1));
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;
        yield return Secondary;

        if (Sugar is not null)
        {
            // need values in reverse order so they play nicely with the later instructions (stack order)
            switch (Sugar.Token.TokenId)
            {
                case CrumpetToken.PLUS:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.ADD);
                    break;
                case CrumpetToken.MINUS:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.SUBTRACT);
                    break;
            }
        }
    }
}