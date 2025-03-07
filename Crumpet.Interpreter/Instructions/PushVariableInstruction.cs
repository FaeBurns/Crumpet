using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Variables;

namespace Crumpet.Interpreter.Instructions;

public class PushVariableInstruction : Instruction
{
    private readonly string m_name;

    public PushVariableInstruction(string name)
    {
        m_name = name;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable var = context.CurrentScope.GetVariable(m_name);
        context.VariableStack.Push(var);
    }
}