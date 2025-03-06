using Crumpet.Language;

namespace Crumpet.Interpreter.Variables.Types;

public class FieldInfo
{
    public FieldInfo(string name, TypeInfo type, VariableModifier variableModifier = VariableModifier.COPY)
    {
        Name = name;
        Type = type;
        VariableModifier = variableModifier;
    }

    public string Name { get; }
    public TypeInfo Type { get; set; }
    public VariableModifier VariableModifier { get; }
}