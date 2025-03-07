using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions;

public class AssignVariableInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable target = context.VariableStack.Pop();
        Variable source = context.VariableStack.Pop();
        target.Value = source;
    }
}