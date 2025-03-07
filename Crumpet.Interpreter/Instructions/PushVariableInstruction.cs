using Crumpet.Interpreter.Variables;

namespace Crumpet.Interpreter.Instructions;

public class PushVariableInstruction : IInstruction
{
    private readonly Variable m_variable;

    public PushVariableInstruction(Variable variable)
    {
        m_variable = variable;
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}