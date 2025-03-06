using System.Diagnostics;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Variables.InstanceValues;

public sealed class InstanceReference
{
    public TypeInfo Type { get; }
    public object Value { get; set; }
    
    public static InstanceReference Create(TypeInfo type, object initialValue) => type.CreateInstance(initialValue);
    public static InstanceReference Create(TypeInfo type) => type.CreateInstance();

    internal InstanceReference(TypeInfo type, object initialValue)
    {
        Type = type;
        Value = initialValue;
    }

    private InstanceReference GetCopy()
    {
        return new InstanceReference(Type, Type.CreateCopy(Value));
    }

    private InstanceReference GetReference()
    {
        return new InstanceReference(Type, Value);
    }

    private InstanceReference GetPointer()
    {
        return this;
    }

    public InstanceReference GetForModifier(VariableModifier modifier)
    {
        return modifier switch
        {
            VariableModifier.COPY => GetCopy(),
            VariableModifier.REFERENCE => GetReference(),
            VariableModifier.POINTER => GetPointer(),
            _ => throw new UnreachableException()
        };
    }
}