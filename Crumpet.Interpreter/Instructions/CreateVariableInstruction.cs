using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Instructions;

public class CreateVariableInstruction : IInstruction
{
    private readonly TypeInfo m_type;
    private readonly string m_name;

    public CreateVariableInstruction(TypeInfo type, string name)
    {
        m_type = type;
        m_name = name;
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}