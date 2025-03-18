using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class DotnetObjectInstance
{
    public DotnetObjectTypeInfo TypeInfo { get; }
    public object? Instance { get; }

    public DotnetObjectInstance(DotnetObjectTypeInfo typeInfo, object? instance)
    {
        TypeInfo = typeInfo;
        Instance = instance;
    }
}