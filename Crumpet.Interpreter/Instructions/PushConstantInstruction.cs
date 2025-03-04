namespace Crumpet.Interpreter.Instructions;

public class PushConstantInstruction : IInstruction
{
    private readonly object m_value;

    public PushConstantInstruction(object value)
    {
        m_value = value;
    }
    
    public void Execute()
    {
        throw new NotImplementedException();
    }
}