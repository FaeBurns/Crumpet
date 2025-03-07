using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Parser;

namespace Crumpet.Interpreter.Functions;

public class FunctionDefinition(string name, TypeInfo returnType, IEnumerable<ParameterDefinition> parameters, SourceLocation sourceLocation)
{
    public string Name { get; } = name;
    public IReadOnlyList<ParameterDefinition> Parameters { get; } = parameters.ToArray();
    public TypeInfo ReturnType { get; } = returnType;
    public SourceLocation SourceLocation { get; } = sourceLocation;
}

public class ParameterDefinition(string name, TypeInfo type, VariableModifier variableModifier)
{
    public string Name { get; } = name;
    public TypeInfo Type { get; } = type;
    public VariableModifier VariableModifier { get; } = variableModifier;
}