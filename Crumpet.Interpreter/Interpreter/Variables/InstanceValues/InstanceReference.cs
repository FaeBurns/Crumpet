using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables.InstanceValues;

public sealed class InstanceReference
{
    public TypeInfo Type { get; }
    public object? Value { get; set; }
    
    public static InstanceReference Create(TypeInfo type, object? value) => new InstanceReference(type, value);
    public static InstanceReference Create(TypeInfo type) => type.CreateInstance();
    
    private InstanceReference(TypeInfo type, object? value)
    {
        Type = type;
        Value = value;
    }
}