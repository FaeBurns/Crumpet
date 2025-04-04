using Crumpet.Exceptions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language.Nodes;
using Shared;

namespace Crumpet.Interpreter;

public class TypeResolver
{
    private readonly Dictionary<string, TypeTemplate> m_templates;
    private readonly Dictionary<int, TypeInfo> m_builtTypeCache = new Dictionary<int, TypeInfo>();
    private readonly Stack<GenericTypeContext> m_genericTypeContextStack = new Stack<GenericTypeContext>();

    internal TypeResolver(IEnumerable<KeyValuePair<string, TypeTemplate>> types)
    {
        m_templates = new Dictionary<string, TypeTemplate>(types);
    }

    public TypeTemplate? ResolveTemplate(string typename)
    {
        return m_templates.GetValueOrDefault(typename);
    }

    public TypeInfo ResolveType(string typeName, IReadOnlyList<TypeInfo> genericPositionalTypeArgs)
    {
        int hash = GetTypeHashCode(typeName, genericPositionalTypeArgs);
        if (m_builtTypeCache.TryGetValue(hash, out TypeInfo? cachedType))
            return cachedType;
        
        if (m_templates.TryGetValue(typeName, out TypeTemplate? typeTemplate))
        {
            // if it's a user type this will end up caching the type itself, so don't force it to be added
            TypeInfo type = typeTemplate.Construct(this, genericPositionalTypeArgs);
            m_builtTypeCache.TryAdd(hash, type);
            return type;
        }

        if (TryResolveGenericNameToType(typeName) is TypeInfo genericType)
        {
            // generic type is resolved here
            return genericType;
        }

        throw new TypeNotFoundException(typeName, ExceptionConstants.UNKOWN_TYPE.Format(typeName));
    }

    public TypeInfo ResolveGenericNameToType(string name)
    {
        return TryResolveGenericNameToType(name) ?? throw new GenericsException(ExceptionConstants.GENERIC_NOT_FOUND.Format(name));
    }

    private TypeInfo? TryResolveGenericNameToType(string name)
    {
        if (!m_genericTypeContextStack.Any())
            return null;
            
        GenericTypeContext currentContext = m_genericTypeContextStack.Peek();
        return currentContext.GetValueOrDefault(name);
    }

    public DisposeAction PushGenericArgumentsAutoPop(GenericTypeContext context)
    {
        m_genericTypeContextStack.Push(context);
        return new DisposeAction(PopGenericArguments);
    }
    
    public void PushGenericArguments(GenericTypeContext context) => m_genericTypeContextStack.Push(context);
    public void PopGenericArguments() => m_genericTypeContextStack.Pop();
    
    public TypeInfo? TryResolveCachedType(TypeTemplate template, IReadOnlyList<TypeInfo> positionalTypeArgs)
    {
        int hash = GetTypeHashCode(template.TypeName, positionalTypeArgs);
        return m_builtTypeCache.GetValueOrDefault(hash);
    }

    public TypeInfo TemplateConstructOrCache(TypeTemplate template, IReadOnlyList<TypeInfo> positionalTypeArgs)
    {
        if (TryResolveCachedType(template, positionalTypeArgs) is TypeInfo typeInfo)
            return typeInfo;

        return template.Construct(this, positionalTypeArgs);
    }
    
    public TypeTemplate TypeNodeToTemplate(TypeNode node)
    {
        if (node.TypeArgs.TypeArguments.Length == 0)
        {
            // if no template was found it must be a generic template
            // e.g. T will not be found by TryGetTemplate so it must be a replaceable generic
            TypeTemplate? template = TryGetTemplate(node.FullName);
            if (template is null)
                return new GenericReplaceableTypeTemplate(node.FullName);
        }

        if (node.TypeArgs.TypeArguments.Length == 0)
            return GetTemplate(node.FullName);
        
        TypeTemplate[] args = new TypeTemplate[node.TypeArgs.TypeArguments.Length];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = TypeNodeToTemplate(node.TypeArgs.TypeArguments[i]);
        }
        
        return new TypeWithTypeArgsTemplate(GetTemplate(node.FullName), args);
    }
    
    public void CacheIncompleteUserType(UserObjectTypeInfo userType, IReadOnlyList<TypeInfo> typeArgs)
    {
        m_builtTypeCache.Add(GetTypeHashCode(userType.TypeName, typeArgs), userType);
    }
    
    private TypeTemplate GetTemplate(string name)
    {
        return m_templates.GetValueOrDefault(name) ?? throw new TypeNotFoundException(name, ExceptionConstants.UNKOWN_TYPE.Format(name));
    }

    private TypeTemplate? TryGetTemplate(string name)
    {
        return m_templates.GetValueOrDefault(name);
    }
    
    private int GetTypeHashCode(string typeName, IReadOnlyList<TypeInfo> args)
    {
        HashCombo hash = new HashCombo()
            .Add(typeName);
        
        foreach (TypeInfo type in args)
        {
            hash.Add(type.GetHashCode());
        }

        return hash.GetHashCode();
    }
}

public class GenericTypeContext : Dictionary<string, TypeInfo>
{
    public GenericTypeContext()
    {
    }
    
    public GenericTypeContext(IEnumerable<KeyValuePair<string, TypeInfo>> typeArgs) : base(typeArgs)
    {
    }
    
    public GenericTypeContext(IReadOnlyList<string> names, IReadOnlyList<TypeInfo> types)
    {
        if (types.Count != names.Count)
            throw new GenericsException(ExceptionConstants.TYPE_RESOLVE_GENERIC_ARG_COUNT_MISMATCH.Format(names.Count, types.Count));

        for (int i = 0; i < names.Count; i++)
        {
            Add(names[i], types[i]);
        }
    }
}