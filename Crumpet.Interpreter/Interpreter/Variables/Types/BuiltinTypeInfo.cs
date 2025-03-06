using System.Diagnostics;
using System.Runtime.CompilerServices;
using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables.Types;

public class BuiltinTypeInfo<T> : TypeInfo, IBuiltinTypeInfo
{
    public override string TypeName => typeof(T).Name;
    
    public override InstanceReference CreateInstance()
    {
        // handle string specifically - trying to avoid nulls
        if (typeof(T) == typeof(string))
            return InstanceReference.Create(this, String.Empty);
        
        // any other than string will not be null with default
        return InstanceReference.Create(this, default(T)!);
    }

    public override InstanceReference CreateInstance(object initialValue)
    {
        if (initialValue is not StrongBox<T> boxedValue)
        {
            if (initialValue is not T value)
                throw new ArgumentException(ExceptionConstants.CREATE_INSTANCE_INVALID_INITIAL_VALUE, nameof(initialValue));
            
            boxedValue = new StrongBox<T>(value);
        }

        return new InstanceReference(this, boxedValue);
    }

    public override bool ConvertableTo(TypeInfo other)
    {
        // int to float conversion
        if (this is BuiltinTypeInfo<int> && other is BuiltinTypeInfo<float>)
            return true;
        
        return base.ConvertableTo(other);
    }

    public override InstanceReference ConvertInstance(InstanceReference instance)
    {
        if (this is BuiltinTypeInfo<float> && instance.Type is BuiltinTypeInfo<int>)
            return InstanceReference.Create(this, (float)(int)instance.Value);
        
        return base.ConvertInstance(instance);
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