using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ExclusiveOrExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public EqualityExpressionNode Primary { get; }
    public EqualityExpressionNode? Secondary { get; }

    public ExclusiveOrExpressionNode(EqualityExpressionNode primary, EqualityExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExclusiveOrExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<EqualityExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.XOR),
                        new NonTerminalConstraint<EqualityExpressionNode>()))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;
        yield return Secondary;
        
        // only push primary if there is no secondary section
        if (Secondary is not null)
            yield return new LogicalBooleanInstruction(LogicalBooleanInstruction.Operation.XOR);
    }
}