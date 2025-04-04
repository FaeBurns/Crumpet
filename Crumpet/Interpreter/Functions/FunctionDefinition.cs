using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class FunctionDefinition(string name, TypeTemplate returnType, VariableModifier returnModifier, IEnumerable<ParameterTemplate> parameters, IReadOnlyList<string> typeParameterNames, SourceLocation sourceLocation)
{
    public string Name { get; } = name;
    public IReadOnlyList<ParameterTemplate> Parameters { get; } = parameters.ToArray();
    public TypeTemplate ReturnType { get; } = returnType;
    public VariableModifier ReturnModifier { get; } = returnModifier;
    public IReadOnlyList<string> TypeParameterNames { get; } = typeParameterNames;
    public SourceLocation SourceLocation { get; } = sourceLocation;
}