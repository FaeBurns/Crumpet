using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Instructions;

public class PushConstantInstruction : Instruction
{
    private readonly TypeInfo m_type;
    private readonly object m_value;

    public PushConstantInstruction(TypeInfo type, object value)
    {
        m_type = type;
        m_value = value;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        context.VariableStack.Push(Variable.Create(m_type, m_value));
    }
}