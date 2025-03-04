using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Instructions;

public class PushInstanceReferenceInstruction : IInstruction
{
    private readonly InstanceReference m_reference;

    public PushInstanceReferenceInstruction(InstanceReference reference)
    {
        m_reference = reference;
    }
    
    public void Execute()
    {
        throw new NotImplementedException();
    }
}