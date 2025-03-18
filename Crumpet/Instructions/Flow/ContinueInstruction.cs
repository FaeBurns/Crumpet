using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions.Flow;

public class ContinueInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        context.Continue();
    }
}