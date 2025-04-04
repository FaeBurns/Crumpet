using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class DirectTypeTemplate : TypeTemplate
{
    public TypeInfo Type { get; }

    public DirectTypeTemplate(TypeInfo type)
    {
        Type = type;
        TypeName = type.TypeName;
    }
    
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        // if it needed them, it would use them, but it doesn't
        // so ignore them
        // if (positionalTypeArguments.Count > 0)
        //     throw new GenericsException(ExceptionConstants.GENERIC_ARGUMENT_COUNT_MISMATCH.Format(0, positionalTypeArguments.Count));

        return Type;
    }
}