using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

public class ObjectInstance
{
    public ObjectTypeInfo Type { get; }

    public ObjectInstance(ObjectTypeInfo type)
    {
        Type = type;
        Fields = new FieldCollection(type.Fields);
    }

    public FieldCollection Fields { get; }
}