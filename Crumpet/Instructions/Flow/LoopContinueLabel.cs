using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class LoopContinueLabel : Instruction
{
    public LoopContinueLabel(SourceLocation location) : base(location)
    {
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
    }
}