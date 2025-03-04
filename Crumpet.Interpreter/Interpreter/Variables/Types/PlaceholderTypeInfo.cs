using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables.Types;

internal class PlaceholderTypeInfo : TypeInfo
{
    public PlaceholderTypeInfo(string typeName)
    {
        TypeName = typeName;
    }

    public override string TypeName { get; }
    public override InstanceReference CreateInstance()
    {
        throw new InvalidOperationException(ExceptionConstants.CREATE_PLACEHOLDER_TYPE);
    }
}