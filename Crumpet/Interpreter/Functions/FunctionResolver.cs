using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;
using Shared.Collections;

namespace Crumpet.Interpreter.Functions;

public class FunctionResolver
{
    private readonly TypeResolver m_typeResolver;
    private readonly MultiDictionary<string, FunctionTemplate> m_functionTemplates = new MultiDictionary<string, FunctionTemplate>();
    private readonly MultiDictionary<string, Function> m_builtFunctionCache = new MultiDictionary<string, Function>();
    
    public FunctionResolver(TypeResolver typeResolver, IEnumerable<Function> functions, IEnumerable<FunctionTemplate> templates)
    {
        m_typeResolver = typeResolver;
        foreach (Function function in functions)
        {
            m_builtFunctionCache.Add(function.Name, function);
        }

        foreach (FunctionTemplate template in templates)
        {
            m_functionTemplates.Add(template.Name, template);
        }
    }

    public Function GetFunction(string functionName, IEnumerable<ParameterInfo> passingParameters, IReadOnlyList<TypeInfo> typeArgs)
    {
        // check in the cache before building from template
        if (m_builtFunctionCache.TryGetValue(functionName, out List<Function>? functions))
        {
            Function? match = functions.FirstOrDefault(f => MatchParameters(f, passingParameters, typeArgs.Count));
            if (match is not null)
                return match;
        }
        
        // search for template
        FunctionTemplate? template = m_functionTemplates.GetValueOrDefault(functionName, f => MatchTemplateParameters(f, passingParameters, typeArgs));
        if (template is null)
            throw new KeyNotFoundException(ExceptionConstants.FUNCTION_NOT_FOUND.Format(functionName, String.Join(", ", passingParameters.Select(p => p.ToString()))));

        return template.Construct(m_typeResolver, typeArgs);
    }

    private bool MatchTemplateParameters(FunctionTemplate template, IEnumerable<ParameterInfo> passingParameters, IReadOnlyList<TypeInfo> typeArgs)
    {
        ParameterTemplate[] funcParamTemplates = template.Parameters.ToArray();
        ParameterInfo[] passingParams = passingParameters.ToArray();

        // false if lengths don't match
        if (funcParamTemplates.Length != passingParams.Length)
            return false;

        // false if type arg count doesn't match
        if (template.TypeParameters.Count != typeArgs.Count)
            return false;
        
        // push generic arguments that will automatically pop when this method is returned
        using DisposeAction action = m_typeResolver.PushGenericArgumentsAutoPop(new GenericTypeContext(template.TypeParameters, typeArgs));

        ParameterInfo[] funcParams = new ParameterInfo[funcParamTemplates.Length];
        for (int i = 0; i < funcParamTemplates.Length; i++)
        {
            // template construct might throw so return false if it does
            try
            {
                TypeInfo paramType = m_typeResolver.TemplateConstructOrCache(funcParamTemplates[i].Template, typeArgs);
                funcParams[i] = new ParameterInfo(paramType, funcParamTemplates[i].Modifier);
            }
            catch
            {
                return false;
            }
        }
        
        // basic equality check
        if (funcParams.SequenceEqual(passingParams))
            return true;
        
        // check for equivalent types
        for (int i = 0; i < funcParams.Length; i++)
        {
            TypeInfo funcParamType = funcParams[i].Type;
            TypeInfo passingParamType = passingParams[i].Type;
            VariableModifier funcParamModifier = funcParams[i].Modifier;
            VariableModifier passingParamModifier = passingParams[i].Modifier;
            
            // modifiers must match
            if (funcParams[i].Modifier != passingParamModifier)
                return false;

            if (funcParams[i].Modifier == VariableModifier.POINTER && passingParamType is NullTypeInfo)
                continue;
            
            // if they're equal
            if (funcParamType == passingParamType)
                continue;
            
            // if the function accepts anything
            if (funcParamType is AnyTypeInfo)
                continue;
            
            // if the function allows any array type
            // exclude specific array types for this check
            if (funcParamType is ArrayTypeInfoUnkownTypeInfo and not ArrayTypeInfo && passingParamType is ArrayTypeInfo)
                continue;
            
            if (funcParamType is TypeTypeInfoUnknownTypeInfo and not TypeTypeInfo && passingParamType is TypeTypeInfo)
                continue;

            // type specific check
            // only do this check if it's a copy type
            if (funcParamType.IsAssignableFrom(passingParamType) && funcParamModifier == VariableModifier.COPY)
                continue;

            // if none of the conditions pass this will be hit
            return false;
        }

        return true;
    }

    private bool MatchParameters(Function function, IEnumerable<ParameterInfo> passingParameters, int typeArgCount)
    {
        ParameterInfo[] funcParams = function.Parameters.Select(p => new ParameterInfo(p.Type, p.Modifier)).ToArray();
        ParameterInfo[] passingParams = passingParameters.ToArray();

        // false if lengths don't match
        if (funcParams.Length != passingParams.Length)
            return false;

        // false if type arg count doesn't match
        if (function.TypeArgCount != typeArgCount)
            return false;
        
        // basic equality check
        if (funcParams.SequenceEqual(passingParams))
            return true;
        
        // check for equivalent types
        for (int i = 0; i < funcParams.Length; i++)
        {
            TypeInfo funcParamType = funcParams[i].Type;
            TypeInfo passingParamType = passingParams[i].Type;
            VariableModifier funcParamModifier = funcParams[i].Modifier;
            VariableModifier passingParamModifier = passingParams[i].Modifier;
            
            // modifiers must match
            if (funcParams[i].Modifier != passingParamModifier)
                return false;

            if (funcParams[i].Modifier == VariableModifier.POINTER && passingParamType is NullTypeInfo)
                continue;
            
            // if they're equal
            if (funcParamType == passingParamType)
                continue;
            
            // if the function accepts anything
            if (funcParamType is AnyTypeInfo)
                continue;
            
            // if the function allows any array type
            // exclude specific array types for this check
            if (funcParamType is ArrayTypeInfoUnkownTypeInfo and not ArrayTypeInfo && passingParamType is ArrayTypeInfo)
                continue;
            
            if (funcParamType is TypeTypeInfoUnknownTypeInfo and not TypeTypeInfo && passingParamType is TypeTypeInfo)
                continue;

            if (funcParamType is DictionaryTypeInfoUnknownType and not DictionaryTypeInfo && passingParamType is DictionaryTypeInfo)
                continue;

            // type specific check
            // only do this check if it's a copy type
            if (funcParamType.IsAssignableFrom(passingParamType) && funcParamModifier == VariableModifier.COPY)
                continue;

            // if none of the conditions pass this will be hit
            return false;
        }

        return true;
    }
}