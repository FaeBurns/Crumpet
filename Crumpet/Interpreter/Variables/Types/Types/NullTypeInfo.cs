using Crumpet.Language;

namespace Crumpet.Interpreter.Variables.Types;

public class NullTypeInfo : TypeInfo
{
    public override string TypeName { get; } = "NULL";

    public static Variable Create() => new NullTypeInfo().CreateVariable();
    
    public override Variable CreateVariable()
    {
        return Variable.Create(this, null);
    }

    public override object CreateCopy(object? instance)
    {
        // the object doesn't have a value so it's fine?
        return instance!;
    }

    public override int GetObjectHashCode(Variable variable)
    {
        return 0;
    }
}