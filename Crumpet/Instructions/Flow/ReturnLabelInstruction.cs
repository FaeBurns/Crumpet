using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class ReturnLabelInstruction : Instruction
{
    public ReturnLabelInstruction(SourceLocation location) : base(location)
    {
    }

    public override void Execute(InterpreterExecutionContext context)
    {
    }
}