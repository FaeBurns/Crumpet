namespace Crumpet.Interpreter.Variables.Types.Templates;

public class PlaceholderTypeTemplate : TypeTemplate
{
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        throw new InvalidOperationException();
    }
}