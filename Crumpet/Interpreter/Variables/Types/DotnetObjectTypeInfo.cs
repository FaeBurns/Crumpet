namespace Crumpet.Interpreter.Variables.Types;

public class DotnetObjectTypeInfo : TypeInfo
{
    public DotnetObjectTypeInfo(Type dotnetType)
    {
        DotnetType = dotnetType;
    }
    
    public Type DotnetType { get; }
    public override string TypeName => DotnetType.FullName!;
    public override Variable CreateVariable()
    {
        // needs to have a zero argument constructor
        return Variable.Create(this, new DotnetObjectInstance(this, null));
    }

    public Variable CreateVariable(object instance)
    {
        // does not need to have a zero argument constructor here
        return Variable.Create(this, new DotnetObjectInstance(this, instance));
    }

    public override object CreateCopy(object instance)
    {
        // this will be called whenever an object of this type is used as an argument
        // throw new InvalidOperationException();
        
        // just return the same instance?
        // we just have to follow the regular C# rules; value types are copied, reference types are not.
        if (DotnetType.IsValueType)
            return (object)(ValueType)instance;
        else
            return instance;
    }
}

public class DotnetObjectTypeInfo<T> : DotnetObjectTypeInfo where T : new()
{
    public DotnetObjectTypeInfo() : base(typeof(T))
    {
    }
}