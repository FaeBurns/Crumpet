using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Instructions.Unary;

public class LogicalNotInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        // get bool value and negate it then push
        context.VariableStack.Push(BuiltinTypeInfo.Bool, !context.VariableStack.Pop().GetValue<bool>());
    }
}