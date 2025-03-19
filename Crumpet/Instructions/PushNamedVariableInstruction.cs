using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;

namespace Crumpet.Instructions;

public class PushNamedVariableInstruction : Instruction
{
    private readonly string m_name;

    public PushNamedVariableInstruction(string name)
    {
        m_name = name;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable var = context.CurrentScope.GetVariable(m_name);
        context.VariableStack.Push(var);
    }
}

public class PushVariableInstruction : Instruction
{
    private readonly Variable m_variable;

    public PushVariableInstruction(Variable variable)
    {
        m_variable = variable;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        context.VariableStack.Push(m_variable);
    }
}