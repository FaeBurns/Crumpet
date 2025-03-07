using Crumpet.Instructions.Boolean;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class OrExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public AndExpressionNode Primary { get; }
    public AndExpressionNode? Secondary { get; }

    public OrExpressionNode(AndExpressionNode primary, AndExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<OrExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<AndExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.OR),
                        new NonTerminalConstraint<AndExpressionNode>()))),
            GetNodeConstructor<OrExpressionNode>());
    }

    public IEnumerable<Instruction> GetInstructions()
    {
        // don't push anything if there is no secondary
        if (Secondary is null)
            yield break;

        yield return new LogicalBooleanInstruction(LogicalBooleanInstruction.Operation.OR);
    }
}