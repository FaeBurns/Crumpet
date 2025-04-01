using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;
using Shared.Collections;

namespace Crumpet.Interpreter.Functions;

public class FunctionResolver
{
    private readonly MultiDictionary<string, Function> m_functions = new MultiDictionary<string, Function>();
    
    public FunctionResolver(IEnumerable<Function> functions)
    {
        foreach (Function function in functions)
        {
            m_functions.Add(function.Name, function);
        }
    }

    public Function GetFunction(string functionName, IEnumerable<ParameterInfo> passingParameters, int typeArgCount)
    {
        return m_functions.GetValueOrDefault(functionName, f => MatchParameters(f, passingParameters, typeArgCount))
               ?? throw new KeyNotFoundException(ExceptionConstants.FUNCTION_NOT_FOUND.Format(functionName, String.Join(", ", passingParameters.Select(p => p.ToString()))));
    }

    private bool MatchParameters(Function function, IEnumerable<ParameterInfo> passingParameters, int typeArgCount)
    {
        ParameterInfo[] funcParams = function.Parameters.ToArray();
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