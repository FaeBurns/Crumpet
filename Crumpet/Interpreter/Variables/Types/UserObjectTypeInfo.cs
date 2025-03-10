﻿using Crumpet.Language;

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

    public override Variable CreateVariable()
    {
        return Variable.Create(this, new UserObjectInstance(this));
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
        // need to assume the instance is of the exact same TypeInfo type
        UserObjectInstance newInstance = new UserObjectInstance(this);

        // perform deep copy on all fields as necessary
        // InstanceReference.Value's setter will check CopyOnAssign and do a recurisve copy that calls this if it needs to
        foreach (string name in objectInstance.Fields.VariableNames)
        {
            // assign field instance based on target field modifier
            VariableModifier modifier = Fields.First(f => f.Name == name).VariableModifier;

            // copy, pointer, or ref assign handled in setter
            // as long as the passed value is a Variable instance itself
            newInstance.Fields[name].Value = objectInstance.Fields[name];
        }

        return newInstance;
    }
}