using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Interpreter.Variables;

public class VariableInfo
{
    public VariableInfo(string name, TypeInfo type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public TypeInfo Type { get; }
}