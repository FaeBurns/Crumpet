using System.Diagnostics;
using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter.Variables.Types;

public class VoidTypeInfo : TypeInfo
{
    public override string TypeName => "void";
    
    public override InstanceReference CreateInstance()
    {
        throw new UnreachableException();
    }
}