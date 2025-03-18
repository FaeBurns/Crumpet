using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Parser.Nodes;

namespace Crumpet.Instructions;

public class DebugReferenceProviderInstruction : Instruction
{
    public IInstructionProvider InstructionProvider { get; }

    public DebugReferenceProviderInstruction(IInstructionProvider instructionProvider)
    {
        InstructionProvider = instructionProvider;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
    }
}