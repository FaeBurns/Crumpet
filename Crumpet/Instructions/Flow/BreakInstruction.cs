using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions.Flow;

public class BreakInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        context.Break();
    }
}