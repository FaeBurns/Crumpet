using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language.Nodes;
using Shared;

namespace Crumpet.Interpreter;

public class TypeResolver
{
    private readonly Dictionary<string, TypeInfo> m_types;

    internal TypeResolver(IEnumerable<KeyValuePair<string, TypeInfo>> types)
    {
        m_types = new Dictionary<string, TypeInfo>(types);
    }

    public TypeInfo? ResolveType(string typeName)
    {
        return m_types.GetValueOrDefault(typeName);
    }
}

public class PlaceholderTypeResolver
{
    private readonly Dictionary<string, TypeInfo> m_types = new Dictionary<string, TypeInfo>();

    public PlaceholderTypeResolver()
    {
        // initialize with default types
        m_types.Add("string", BuiltinTypeInfo.String);
        m_types.Add("int", BuiltinTypeInfo.Int);
        m_types.Add("float", BuiltinTypeInfo.Float);
        m_types.Add("bool", BuiltinTypeInfo.Bool);
        m_types.Add("void", new VoidTypeInfo());
        m_types.Add("map", new DictionaryTypeInfoUnknownType());
    }
    
    public void RegisterType(TypeDeclarationNode node)
    {
        IEnumerable<FieldInfo> fieldTypes = node.Fields.Select(f => new FieldInfo(f.Name.Terminal, ResolveType(f.Type.FullName), f.VariableModifier));

        // use direct assign when setting in dictionary to replace placeholders when encountered
        UserObjectTypeInfo typeInfo = new UserObjectTypeInfo(node.Name.Terminal, fieldTypes.ToArray());
        m_types[node.Name.Terminal] = typeInfo;
    }

    /// <summary>
    /// Tries to replace all placeholders.
    /// </summary>
    /// <returns>True if all were replaced, false if not.</returns>
    public bool ReplacePlaceholders()
    {
        bool clean = true;
        foreach (KeyValuePair<string, TypeInfo> type in m_types)
        {
            // replace if placeholder
            if (type.Value is PlaceholderTypeInfo)
                ReplacePlaceholder(type.Key, type.Value);
            
            // invalidate if type is still a placeholder
            if (type.Value is PlaceholderTypeInfo) clean = false;
        }

        return clean;
    }

    public void UpdateFieldsInUserTypes()
    {
        // loop through all fields in all user types
        foreach (FieldInfo field in m_types.Values.OfType<UserObjectTypeInfo>().SelectMany(t => t.Fields))
        {
            // try and replace if it's a placeholder
            if (field.Type is PlaceholderTypeInfo)
                // resolve type will return the same placeholder if nothing has been updated yet
                field.Type = ResolveType(field.Type.TypeName);
        }
    }

    /// <summary>
    /// Gets the resolver from the built type references
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TypeResolver Build()
    {
        foreach (FieldInfo field in m_types.Values.OfType<UserObjectTypeInfo>().SelectMany(t => t.Fields))
        {
            // try and replace if it's a placeholder
            if (field.Type is PlaceholderTypeInfo)
                throw new InvalidOperationException(ExceptionConstants.PLACEHOLDER_STILL_PRESENT.Format(field.Type));
        }

        return new TypeResolver(m_types);
    }
    
    private void ReplacePlaceholder(string typeName, TypeInfo typeInfo)
    {
        // if type not found by name, throw
        if (!m_types.TryGetValue(typeName, out TypeInfo? oldValue))
            throw new ArgumentException(ExceptionConstants.KEY_NOT_FOUND.Format(typeName));
        
        if (oldValue is not PlaceholderTypeInfo)
            throw new InvalidOperationException(ExceptionConstants.REPLACING_NON_PLACEHOLDER_TYPE);
        
        // replace in map
        m_types[typeName] = typeInfo;
    }

    public TypeInfo ResolveType(string typeName)
    {
        // add if missing then return contained value
        m_types.TryAdd(typeName, new PlaceholderTypeInfo(typeName));
        return m_types[typeName];
    }
}