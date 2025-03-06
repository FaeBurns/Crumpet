using System.Diagnostics;
using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables.Types;

public class VoidTypeInfo : TypeInfo
{
    public override string TypeName => "void";
    
    public override InstanceReference CreateInstance()
    {
        throw new UnreachableException();
    }

    public override InstanceReference CreateInstance(object initialValue)
    {
        throw new UnreachableException();
    }

    public override object CreateCopy(object instance)
    {
        throw new UnreachableException();
    }
}