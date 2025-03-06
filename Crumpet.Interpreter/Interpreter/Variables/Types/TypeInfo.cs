using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables.Types;

public abstract class TypeInfo
{
    public override int GetHashCode()
    {
        return TypeName.GetHashCode();
    }

    public abstract string TypeName { get; }

    public abstract InstanceReference CreateInstance();
    public abstract InstanceReference CreateInstance(object initialValue);
    
    public override string ToString()
    {
        return TypeName;
    }
    
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
    
    public static bool operator == (TypeInfo a, TypeInfo b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(TypeInfo a, TypeInfo b)
    {
        return !(a == b);
    }

    public virtual bool ConvertableTo(TypeInfo other) => false;

    public virtual InstanceReference ConvertInstance(InstanceReference instance) => throw new NotImplementedException();
    
    public abstract object CreateCopy(object instance);
}