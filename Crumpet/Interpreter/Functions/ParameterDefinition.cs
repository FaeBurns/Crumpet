using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Functions;

public class ParameterDefinition(string name, TypeInfo type, VariableModifier modifier)
{
    public string Name { get; } = name;
    public TypeInfo Type { get; } = type;
    public VariableModifier Modifier { get; } = modifier;
}