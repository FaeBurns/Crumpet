using TypeInfo = Crumpet.Interpreter.Variables.Types.TypeInfo;

namespace Crumpet.Interpreter;

public class FunctionDefinition(string name, TypeInfo returnType, IEnumerable<ParameterDefinition> parameters)
{
    public string Name { get; } = name;
    public IReadOnlyList<ParameterDefinition> Parameters { get; } = parameters.ToArray();
    public TypeInfo ReturnType { get; } = returnType;
}

public class ParameterDefinition(string name, TypeInfo type)
{
    public string Name { get; } = name;
    public TypeInfo Type { get; } = type;
}