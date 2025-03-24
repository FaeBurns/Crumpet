using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class FunctionDefinition(string name, TypeInfo returnType, VariableModifier returnModifier, IEnumerable<ParameterDefinition> parameters, SourceLocation sourceLocation)
{
    public string Name { get; } = name;
    public IReadOnlyList<ParameterDefinition> Parameters { get; } = parameters.ToArray();
    public TypeInfo ReturnType { get; } = returnType;
    public VariableModifier ReturnModifier { get; } = returnModifier;
    public SourceLocation SourceLocation { get; } = sourceLocation;
}

public class ParameterDefinition(string name, TypeInfo type, VariableModifier variableModifier)
{
    public string Name { get; } = name;
    public TypeInfo Type { get; } = type;
    public VariableModifier VariableModifier { get; } = variableModifier;
}