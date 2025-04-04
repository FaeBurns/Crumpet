using Crumpet.Language;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class FieldTemplate
{
    public string Name { get; }
    public TypeTemplate Type { get; }
    public VariableModifier Modifier { get; }

    public FieldTemplate(string name, TypeTemplate type, VariableModifier modifier)
    {
        Name = name;
        Type = type;
        Modifier = modifier;
    }

    public FieldInfo Resolve(TypeResolver resolver, IReadOnlyList<TypeInfo> objectPositionalTypeArgs)
    {
        TypeInfo fieldType = resolver.TemplateConstructOrCache(Type, objectPositionalTypeArgs);

        return new FieldInfo(Name, fieldType, Modifier);
    }
}