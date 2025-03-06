using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Variables;

public class Variable
{
    private InstanceReference m_instance;
    
    public Variable(string name, InstanceReference instance, VariableModifier variableModifier)
    {
        Name = name;
        m_instance = instance;
        VariableModifier = variableModifier;
    }
    
    public string Name { get; }
    public TypeInfo Type => Instance.Type;

    public InstanceReference Instance
    {
        get => m_instance;
        set => m_instance = value.GetForModifier(VariableModifier);
    }

    public VariableModifier VariableModifier { get; }
}