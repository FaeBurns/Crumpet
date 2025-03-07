using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class UserObjectInstance
{
    public UserObjectInstance(UserObjectTypeInfo type)
    {
        Fields = new VariableCollection();

        foreach (FieldInfo field in type.Fields)
        {
            Fields.Create(new VariableInfo(field.Name, field.Type, field.VariableModifier));
        }
    }

    public VariableCollection Fields { get; }
}