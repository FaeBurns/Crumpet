using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Crumpet.Interpreter.Variables.Types;

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

    public override object ConvertValidObject(TypeInfo type, object value)
    {
        if (this is BuiltinTypeInfo<float> && type is BuiltinTypeInfo<int>)
            return (float)value;

        return base.ConvertValidObject(type, value);
    }

    public override object CreateCopy(object instance)
    {
        // re-box a value type
        // if (instance.GetType().IsValueType)
        //     return Convert.ChangeType(instance, instance.GetType());
        //
        // throw new UnreachableException();

        return (object)(T)instance;
    }
}

public interface IBuiltinTypeInfo
{
}