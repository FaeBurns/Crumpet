using Crumpet.Interpreter.Variables.Types;
using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language;

namespace Crumpet.Interpreter.Functions;

public class ParameterTemplate(string name, TypeTemplate template, VariableModifier modifier)
{
    public string Name { get; } = name;
    public TypeTemplate Template { get; } = template;
    public VariableModifier Modifier { get; } = modifier;

    public override string ToString()
    {
        return "Template " + Template.TypeName + (Modifier == VariableModifier.COPY ? "" : "*");
    }
}