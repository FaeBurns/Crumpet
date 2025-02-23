using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter.Variables.Types;

public abstract class TypeInfo : IComparable<TypeInfo>
{
    public override int GetHashCode()
    {
        return TypeName.GetHashCode();
    }

    public abstract string TypeName { get; }

    public abstract InstanceReference CreateInstance();
    
    protected bool Equals(TypeInfo other)
    {
        return TypeName == other.TypeName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TypeInfo)obj);
    }

    public int CompareTo(TypeInfo? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return string.Compare(TypeName, other.TypeName, StringComparison.Ordinal);
    }
    
    public static bool operator == (TypeInfo a, TypeInfo b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(TypeInfo a, TypeInfo b)
    {
        return !(a == b);
    }
}