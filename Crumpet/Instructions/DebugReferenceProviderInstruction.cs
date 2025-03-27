using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Parser.Nodes;
using Shared;

namespace Crumpet.Instructions;

public class DebugReferenceProviderInstruction : Instruction
{
    public IInstructionProvider InstructionProvider { get; }

    public DebugReferenceProviderInstruction(IInstructionProvider instructionProvider, SourceLocation location) : base(location)
    {
        InstructionProvider = instructionProvider;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
    }
}