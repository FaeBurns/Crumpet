using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables.Types;

public class UserObjectTypeInfo : TypeInfo
{
    public UserObjectTypeInfo(string typeName, params FieldInfo[] fields)
    {
        TypeName = typeName;
        Fields = fields;
    }
    
    public override string TypeName { get; }

    public FieldInfo[] Fields { get; }
    
    public override InstanceReference CreateInstance()
    {
        return InstanceReference.Create(this, new UserObjectInstance(this));
    }

    public override string ToString()
    {
        return TypeName + " {" + String.Join(", ", Fields.Select(f => f.Type.TypeName)) + "}";
    }
}