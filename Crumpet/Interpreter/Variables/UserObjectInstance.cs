using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class UserObjectInstance
{
    private readonly UserObjectTypeInfo m_type;

    public UserObjectInstance(UserObjectTypeInfo type)
    {
        m_type = type;
        Fields = new VariableCollection();

        foreach (FieldInfo field in type.Fields)
        {
            Fields.Create(new VariableInfo(field.Name, field.Type, field.VariableModifier));
        }
    }

    public VariableCollection Fields { get; }

    public override string ToString()
    {
        return m_type.TypeName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UserObjectInstance)obj);
    }

    protected bool Equals(UserObjectInstance other)
    {
        return m_type.Equals(other.m_type) && Fields.Equals(other.Fields);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(m_type, Fields);
    }
}