namespace Crumpet.Interpreter.Variables.Types;

public class VoidTypeInfo : TypeInfo
{
    public override string TypeName => "void";

    public override Variable CreateVariable()
    {
        throw new InvalidOperationException();
    }

    public override object CreateCopy(object instance)
    {
        throw new InvalidOperationException();
    }
}