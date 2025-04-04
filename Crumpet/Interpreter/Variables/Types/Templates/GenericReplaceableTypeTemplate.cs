using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class GenericReplaceableTypeTemplate : TypeTemplate
{
    public string GenericTypeName { get; }

    public GenericReplaceableTypeTemplate(string genericTypeName)
    {
        GenericTypeName = genericTypeName;
    }
    
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        return resolver.ResolveGenericNameToType(GenericTypeName);
    }
}