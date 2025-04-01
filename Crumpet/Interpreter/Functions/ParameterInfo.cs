using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Functions;

public class ParameterInfo(TypeInfo type, VariableModifier modifier)
{
    public TypeInfo Type { get; } = type;
    public VariableModifier Modifier { get; } = modifier;

    public override string ToString()
    {
        return Type.TypeName + (Modifier == VariableModifier.COPY ? "" : "*");
    }
}