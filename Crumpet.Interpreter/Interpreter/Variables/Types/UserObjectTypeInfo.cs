using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Language;

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

    public override InstanceReference CreateInstance(object initialValue)
    {
        if (initialValue is UserObjectInstance userObjectInstance)
            return new InstanceReference(this, userObjectInstance);

        throw new ArgumentException(ExceptionConstants.CREATE_INSTANCE_INVALID_INITIAL_VALUE);
    }

    public override string ToString()
    {
        return TypeName + " {" + String.Join(", ", Fields.Select(f => f.Type.TypeName)) + "}";
    }

    public override object CreateCopy(object instance)
    {
        // cast should always succeed
        UserObjectInstance objectInstance = (UserObjectInstance)instance;
        
        // create new copy instance
        UserObjectInstance newInstance = new UserObjectInstance(objectInstance.Type);
        
        // perform deep copy on all fields as necessary
        // InstanceReference.Value's setter will check CopyOnAssign and do a recurisve copy that calls this if it needs to
        foreach (string name in objectInstance.Fields.VariableNames)
        {
            // assign field instance based on target field modifier
            VariableModifier modifier = Fields.First(f => f.Name == name).VariableModifier;
            // copy, pointer, or ref assign handled in setter
            newInstance.Fields[name].Instance = objectInstance.Fields[name].Instance;
        }

        return newInstance;
    }
}