using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables.InstanceValues;

public class UserObjectInstance
{
    public UserObjectTypeInfo Type { get; }

    public UserObjectInstance(UserObjectTypeInfo type)
    {
        Type = type;
        Fields = new FieldCollection(type.Fields);
    }

    public FieldCollection Fields { get; }
}