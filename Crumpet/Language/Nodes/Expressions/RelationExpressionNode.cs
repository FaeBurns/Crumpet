using Crumpet.Instructions;
using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class RelationExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public SumExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public SumExpressionNode? Secondary { get; }

    public RelationExpressionNode(SumExpressionNode primary, TerminalNode<CrumpetToken> sugar, SumExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public RelationExpressionNode(SumExpressionNode primary) : base(primary)
    {
        Primary = primary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // relation
        yield return new NonTerminalDefinition<RelationExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<SumExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.LESS),
                        new CrumpetTerminalConstraint(CrumpetToken.LESS_OR_EQUAL),
                        new CrumpetTerminalConstraint(CrumpetToken.GREATER_OR_EQUAL),
                        new CrumpetTerminalConstraint(CrumpetToken.GREATER)),
                    new NonTerminalConstraint<SumExpressionNode>())),
            GetNodeConstructor<RelationExpressionNode>(3));

        // passthrough
        yield return new NonTerminalDefinition<RelationExpressionNode>(
                new NonTerminalConstraint<SumExpressionNode>(),
            GetNodeConstructor<RelationExpressionNode>(1));
    }

    public IEnumerable GetInstructionsRecursive()
    {
        // evaluation order is important
        yield return Primary;
        yield return Secondary;

        if (Sugar is not null)
        {
            // need values in reverse order so they play nicely with the later instructions (stack order)
            switch (Sugar.Token.TokenId)
            {
                case CrumpetToken.LESS:
                    yield return new RelationalInstruction(RelationalInstruction.Operation.LESS);
                    break;
                case CrumpetToken.LESS_OR_EQUAL:
                    yield return new RelationalInstruction(RelationalInstruction.Operation.LESS);
                    break;
                case CrumpetToken.GREATER:
                    yield return new RelationalInstruction(RelationalInstruction.Operation.GREATER);
                    break;
                case CrumpetToken.GREATER_OR_EQUAL:
                    yield return new RelationalInstruction(RelationalInstruction.Operation.GREATER_OR_EQUAL);
                    break;
            }
        }
    }
}