using System.Diagnostics;
using Shared;

namespace Crumpet.Interpreter.Variables.Types;

internal class PlaceholderTypeInfo : TypeInfo
{
    public PlaceholderTypeInfo(string typeName)
    {
        TypeName = typeName;
    }

    public override string TypeName { get; }
    public override Variable CreateVariable()
    {
        throw new InvalidOperationException(ExceptionConstants.CREATE_PLACEHOLDER_TYPE);
    }

    public override object CreateCopy(object? instance)
    {
        throw new UnreachableException();
    }
}