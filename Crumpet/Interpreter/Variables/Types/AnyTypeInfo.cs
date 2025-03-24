namespace Crumpet.Interpreter.Variables.Types;

public class AnyTypeInfo : TypeInfo
{
    public override string TypeName => "Any";
    public override Variable CreateVariable()
    {
        throw new InvalidOperationException();
    }

    public override object CreateCopy(object? instance)
    {
        throw new InvalidOperationException();
    }
}