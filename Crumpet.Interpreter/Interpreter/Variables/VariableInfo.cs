using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class VariableInfo
{
    public VariableInfo(string name, TypeInfo type, bool isReference = false)
    {
        Name = name;
        Type = type;
        IsReference = isReference;
    }

    public string Name { get; }
    public TypeInfo Type { get; }
    public bool IsReference { get; }
}