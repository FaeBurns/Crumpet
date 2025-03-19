using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions;

public class ExecuteBuiltInFunctionInstruction : Instruction
{
    private readonly Action<InterpreterExecutionContext> m_function;

    public ExecuteBuiltInFunctionInstruction(Action<InterpreterExecutionContext> function)
    {
        m_function = function;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        m_function.Invoke(context);
    }
}