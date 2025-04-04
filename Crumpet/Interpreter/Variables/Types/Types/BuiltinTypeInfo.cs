using Shared;

namespace Crumpet.Interpreter.Variables.Types;

// used to provide constants
public abstract class BuiltinTypeInfo
{
    public static readonly BuiltinTypeInfo<string> String = new BuiltinTypeInfo<string>();
    public static readonly BuiltinTypeInfo<int> Int = new BuiltinTypeInfo<int>();
    public static readonly BuiltinTypeInfo<bool> Bool = new BuiltinTypeInfo<bool>();
    public static readonly BuiltinTypeInfo<float> Float = new BuiltinTypeInfo<float>();
}

public class BuiltinTypeInfo<T> : TypeInfo, IBuiltinTypeInfo
{
    public override string TypeName => typeof(T).Name;

    public override Variable CreateVariable()
    {
        // handle string specifically - trying to avoid nulls
        if (typeof(T) == typeof(string))
            return Variable.Create(this, String.Empty);

        // any other than string will not be null with default
        return Variable.Create(this, default(T)!);
    }

    public override bool ConvertableTo(TypeInfo other)
    {
        // int to float conversion
        if (this is BuiltinTypeInfo<int> && other is BuiltinTypeInfo<float>)
            return true;

        return base.ConvertableTo(other);
    }

    public override object? ConvertValidObjectTo(TypeInfo type, object? value)
    {
        if (value is null)
            throw new NullReferenceException();
        
        if (this is BuiltinTypeInfo<int> && type is BuiltinTypeInfo<float>)
            return (float)(int)value;

        return base.ConvertValidObjectTo(type, value);
    }

    public override object CreateCopy(object? instance)
    {
        // re-box a value type
        // if (instance.GetType().IsValueType)
        //     return Convert.ChangeType(instance, instance.GetType());
        //
        // throw new UnreachableException();
        
        return (object)(T)(instance ?? throw new NullReferenceException());
    }

    public override int GetObjectHashCode(Variable variable)
    {
        return variable.DereferenceOrGetValue()?.GetHashCode() ?? 0;
    }
}

public interface IBuiltinTypeInfo
{
}