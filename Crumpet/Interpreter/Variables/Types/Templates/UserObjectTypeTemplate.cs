using Crumpet.Exceptions;
using Shared;

namespace Crumpet.Interpreter.Variables.Types.Templates;

public class UserObjectTypeTemplate : TypeTemplate
{
    public IReadOnlyList<string> GenericParameters { get; }
    public FieldTemplate[] Fields { get; internal set; }

    public UserObjectTypeTemplate(string typeName, IReadOnlyList<string> genericParameters, FieldTemplate[] fields)
    {
        GenericParameters = genericParameters;
        Fields = fields;
        TypeName = typeName;
    }
    
    public override TypeInfo Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> positionalTypeArguments)
    {
        if (positionalTypeArguments.Count != GenericParameters.Count)
            throw new GenericsException(ExceptionConstants.GENERIC_ARGUMENT_COUNT_MISMATCH.Format(GenericParameters.Count, positionalTypeArguments.Count));
        
        // try and get from cache to avoid building the type again
        TypeInfo? cached = resolver.TryResolveCachedType(this, positionalTypeArguments);
        if (cached is not null)
            return cached;

        // create and cache it so fields can use this type without causing a stack overflow
        UserObjectTypeInfo userType = new UserObjectTypeInfo(TypeName, positionalTypeArguments.ToArray(), []);
        resolver.CacheIncompleteUserType(userType, positionalTypeArguments);
        
        resolver.PushGenericArguments(new GenericTypeContext(GenericParameters, positionalTypeArguments));
        
        // resolve all fields and create user object from it
        FieldInfo[] fields = Fields.Select(f => f.Resolve(resolver, positionalTypeArguments)).ToArray();
        
        // pop after calling resolve on fields
        resolver.PopGenericArguments();

        userType.Fields = fields.ToArray();
        return userType;
    }
}