namespace Crumpet.Interpreter.Variables.Types;

public class TypeTypeInfo : TypeTypeInfoUnknownTypeInfo
{
    public TypeInfo Type { get; }

    public override string TypeName => "Type: " + Type.TypeName;
    
    public TypeTypeInfo(TypeInfo type)
    {
        Type = type;
    }
    
    public override Variable CreateVariable()
    {
        return Variable.Create(this, Type);
    }

    public override object CreateCopy(object? instance)
    {
        // not actually creating a copy but tbh it should be fine for this?
        return Type;
    }
}

public class TypeTypeInfoUnknownTypeInfo : TypeInfo
{
    public override string TypeName => "Type: ?";
    public override Variable CreateVariable()
    {
        throw new InvalidOperationException();
    }

    public override object CreateCopy(object? instance)
    {
        throw new InvalidOperationException();
    }
}