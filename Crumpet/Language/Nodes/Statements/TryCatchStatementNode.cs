using Crumpet.Instructions;
using Crumpet.Instructions.Details;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class TryCatchStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public StatementBodyNode TryBody { get; }
    public StatementBodyNode CatchBody { get; }
    public IdentifierNode? CatchMessageVariableName { get; }

    public TryCatchStatementNode(StatementBodyNode tryBody, IdentifierNode? catchMessageVariableName, StatementBodyNode catchBody)
    {
        TryBody = tryBody;
        CatchMessageVariableName = catchMessageVariableName;
        CatchBody = catchBody;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TryCatchStatementNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_TRY),
                new NonTerminalConstraint<StatementBodyNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_CATCH),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<TryCatchStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        // encapsulate everything inside a unit
        yield return new ExecuteUnitInstruction(new InstructionCollator(OuterBlockInstructions()), Location);
    }

    private IEnumerable OuterBlockInstructions()
    {
        Guid skipCatchGuid = Guid.NewGuid();
        StackItemCounter counter = new StackItemCounter();

        // save stack counter
        yield return new SaveStackItemCountInstruction(counter, Location);
        
        // body of the try portion
        // if an exception is thrown during this then execution will skip to the CatchLabelInstruction
        yield return new ExecuteUnitInstruction(new InstructionCollator(TryBody), Location);

        // if this is hit then no exception was thrown so jump past the catch portion directly to the end
        yield return new JumpInstruction(skipCatchGuid, Location);
        
        // catch label
        // set the name if one was provided
        yield return new CatchInstruction(CatchMessageVariableName?.Terminal, Location);
        
        // unwind stack to count
        yield return new RestoreStackItemCountInstruction(counter, Location);

        // body of the catch portion
        yield return CatchBody;
        
        // jump target to skip the catch
        yield return new LabelInstruction(skipCatchGuid, Location) { FriendlyName = "Catch skip" };
    }
}