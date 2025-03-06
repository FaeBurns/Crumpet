using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables.InstanceValues;

public class UserObjectInstance
{
    public UserObjectTypeInfo Type { get; }

    public UserObjectInstance(UserObjectTypeInfo type)
    {
        Type = type;
        Fields = new VariableCollection();
        
        foreach (FieldInfo field in type.Fields)
        {
            Fields.Create(new VariableInfo(field.Name, field.Type, field.VariableModifier));
        }
    }

    public VariableCollection Fields { get; }
}