using Crumpet.Interpreter.Variables.Types;
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

    public Function GetFunction(string functionName, IEnumerable<TypeInfo> passingParameters)
    {
        return m_functions.GetValueOrDefault(functionName, f => MatchParameters(f.ParameterTypes, passingParameters))
               ?? throw new KeyNotFoundException(ExceptionConstants.FUNCTION_NOT_FOUND.Format(functionName, String.Join(", ", passingParameters.Select(p => p.ToString()))));
    }

    private bool MatchParameters(IEnumerable<TypeInfo> functionParameters, IEnumerable<TypeInfo> passingParameters)
    {
        TypeInfo[] funcParams = functionParameters.ToArray();
        TypeInfo[] passingParams = passingParameters.ToArray();

        // false if lengths don't match
        if (funcParams.Length != passingParams.Length)
            return false;
        
        // basic equality check
        if (funcParams.SequenceEqual(passingParams))
            return true;
        
        // check for equivalent types
        for (int i = 0; i < funcParams.Length; i++)
        {
            // if they're equal
            if (funcParams[i] == passingParams[i])
                continue;
            
            // if the function accepts anything
            if (funcParams[i] is AnyTypeInfo)
                continue;
            
            // if the function allows any array type
            // exclude specific array types for this check
            if (funcParams[i] is ArrayTypeInfoUnkownType and not ArrayTypeInfo && passingParams[i] is ArrayTypeInfo)
                continue;
            
            if (funcParams[i] is TypeTypeInfoUnknownType and not TypeTypeInfo && passingParams[i] is TypeTypeInfo)
                continue;

            // if none of the conditions pass this will be hit
            return false;
        }

        return true;
    }
}