namespace Crumpet.Interpreter.Variables.Types.Templates;

public abstract class TypeTemplate
{
    public string TypeName { get; protected init; } = String.Empty;

    public abstract TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments);
}