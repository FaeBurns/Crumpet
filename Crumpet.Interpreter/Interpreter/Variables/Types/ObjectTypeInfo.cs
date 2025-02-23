using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter.Variables.Types;

public class ObjectTypeInfo : TypeInfo
{
    public ObjectTypeInfo(string typeName, params VariableInfo[] fields)
    {
        TypeName = typeName;
        Fields = fields;
    }
    
    public override string TypeName { get; }

    public VariableInfo[] Fields { get; }
    
    public override InstanceReference CreateInstance()
    {
        return InstanceReference.Create(this, new ObjectInstance(this));
    }
}