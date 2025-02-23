using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter.Variables.Types;

public class BuiltinTypeInfo<T> : TypeInfo
{
    public override string TypeName => typeof(T).Name;
    
    public override InstanceReference CreateInstance()
    {
        // handle string specifically - trying to avoid nulls
        if (typeof(T) == typeof(string))
            return InstanceReference.Create(this, String.Empty);
        
        return InstanceReference.Create(this, default(T));
    }
}