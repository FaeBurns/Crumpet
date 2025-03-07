namespace Crumpet.Interpreter.Variables.Types;

public abstract class TypeInfo
{
    public override int GetHashCode()
    {
        return TypeName.GetHashCode();
    }

    public abstract string TypeName { get; }

    public abstract Variable CreateVariable();

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

    public virtual object ConvertValidObject(TypeInfo type, object value) => throw new NotImplementedException();

    public abstract object CreateCopy(object instance);

    public bool IsAssignableTo(TypeInfo other)
    {
        return this == other || ConvertableTo(other);
    }

    public bool IsAssignableFrom(TypeInfo other)
    {
        return other.IsAssignableTo(this);
    }
}