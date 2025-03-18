using Crumpet.Instructions.Binary;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class AndExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public ExclusiveOrExpressionNode Primary { get; }
    public ExclusiveOrExpressionNode? Secondary { get; }

    public AndExpressionNode(ExclusiveOrExpressionNode primary, ExclusiveOrExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<AndExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ExclusiveOrExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.AND_AND),
                        new NonTerminalConstraint<ExclusiveOrExpressionNode>()))),
            GetNodeConstructor<AndExpressionNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;
        yield return Secondary;
        
        if (Secondary is not null)
            yield return new LogicalBooleanInstruction(LogicalBooleanInstruction.Operation.AND);
    }
}