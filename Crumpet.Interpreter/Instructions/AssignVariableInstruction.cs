using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Variables;

namespace Crumpet.Interpreter.Instructions;

public class AssignVariableInstruction : Instruction
{
    private readonly Variable m_variable;

    public AssignVariableInstruction(Variable variable)
    {
        m_variable = variable;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable var = context.VariableStack.Pop();
        m_variable.Value = var;
    }
}