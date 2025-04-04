namespace Crumpet.Interpreter.Variables.Types.Templates;

public class TypeWithTypeArgsTemplate : TypeTemplate
{
    public TypeTemplate Template { get; }
    public IReadOnlyList<TypeTemplate> TypeArgs { get; }

    public TypeWithTypeArgsTemplate(TypeTemplate template, IReadOnlyList<TypeTemplate> typeArgs)
    {
        Template = template;
        TypeArgs = typeArgs;
        TypeName = template.TypeName;
    }
    
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        TypeInfo[] resolvedArgs = new TypeInfo[TypeArgs.Count];
        for (int i = 0; i < TypeArgs.Count; i++)
        {
            resolvedArgs[i] = resolver.TemplateConstructOrCache(TypeArgs[i], positionalTypeArguments);
        }
        
        return resolver.TemplateConstructOrCache(Template, resolvedArgs);
    }
}