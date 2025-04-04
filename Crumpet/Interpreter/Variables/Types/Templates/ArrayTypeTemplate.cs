using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class ArrayTypeTemplate : TypeTemplate
{
    private readonly TypeTemplate m_elementType;

    public ArrayTypeTemplate(TypeTemplate elementType)
    {
        m_elementType = elementType;
        TypeName = "Array<>";
    }
    
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        return new ArrayTypeInfo(resolver.TemplateConstructOrCache(m_elementType, positionalTypeArguments));
    }
}