using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Variables;

public class VariableInfo
{
    public VariableInfo(string name, TypeInfo type, VariableModifier variableModifier = VariableModifier.COPY)
    {
        Name = name;
        Type = type;
        VariableModifier = variableModifier;
    }

    public VariableInfo(ParameterDefinition parameterDefinition)
    {
        Name = parameterDefinition.Name;
        Type = parameterDefinition.Type;
        VariableModifier = parameterDefinition.VariableModifier;
    }

    public string Name { get; }
    public TypeInfo Type { get; }
    public VariableModifier VariableModifier { get; }
}