using Crumpet.Interpreter.Variables;

namespace Crumpet.Interpreter;

public class TypeDefinition(string name, IEnumerable<VariableInfo> fields)
{
    public string Name { get; } = name;
    public VariableInfo[] Fields { get; } = fields.ToArray();
}