using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions.Flow;

public class PushReturnValueInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        context.VariableStack.Push(context.LatestReturnValue!);
    }
}