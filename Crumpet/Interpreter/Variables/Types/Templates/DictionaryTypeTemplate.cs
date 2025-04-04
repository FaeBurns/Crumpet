using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class DictionaryTypeTemplate : TypeTemplate
{
    public DictionaryTypeTemplate()
    {
        TypeName = "Dictionary<,>";
    }

    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        if (positionalTypeArguments.Count != 2)
            throw new GenericsException(ExceptionConstants.GENERIC_ARGUMENT_COUNT_MISMATCH.Format(2, positionalTypeArguments.Count));

        return new DictionaryTypeInfo(positionalTypeArguments[0], positionalTypeArguments[1]);
    }
}