using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter.Variables;

public class FieldCollection
{
    private readonly Dictionary<string, InstanceReference> m_fields;
    
    public FieldCollection(VariableInfo[] fields)
    {
        m_fields = new Dictionary<string, InstanceReference>(
            fields.Select(
                f => new KeyValuePair<string, InstanceReference>
                    (f.Name, f.Type.CreateInstance())
            )
        );
    }

    public InstanceReference this[string fieldName]
    {
        get => m_fields[fieldName];
        set => m_fields[fieldName] = value;
    }

    public bool Has(string name)
    {
        return m_fields.ContainsKey(name);
    }
}