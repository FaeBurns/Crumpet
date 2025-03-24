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

    public Function GetFunction(string functionName, IEnumerable<ParameterInfo> passingParameters)
    {
        return m_functions.GetValueOrDefault(functionName, f => MatchParameters(f.Parameters, passingParameters))
               ?? throw new KeyNotFoundException(ExceptionConstants.FUNCTION_NOT_FOUND.Format(functionName, String.Join(", ", passingParameters.Select(p => p.ToString()))));
    }

    private bool MatchParameters(IEnumerable<ParameterInfo> functionParameters, IEnumerable<ParameterInfo> passingParameters)
    {
        ParameterInfo[] funcParams = functionParameters.ToArray();
        ParameterInfo[] passingParams = passingParameters.ToArray();

        // false if lengths don't match
        if (funcParams.Length != passingParams.Length)
            return false;
        
        // basic equality check
        if (funcParams.SequenceEqual(passingParams))
            return true;
        
        // check for equivalent types
        for (int i = 0; i < funcParams.Length; i++)
        {
            // modifiers must match
            if (funcParams[i].Modifier != passingParams[i].Modifier)
                return false;

            if (funcParams[i].Modifier == VariableModifier.POINTER && passingParams[i].Type is NullTypeInfo)
                continue;
            
            // if they're equal
            if (funcParams[i].Type == passingParams[i].Type)
                continue;
            
            // if the function accepts anything
            if (funcParams[i].Type is AnyTypeInfo)
                continue;
            
            // if the function allows any array type
            // exclude specific array types for this check
            if (funcParams[i].Type is ArrayTypeInfoUnkownType and not ArrayTypeInfo && passingParams[i].Type is ArrayTypeInfo)
                continue;
            
            if (funcParams[i].Type is TypeTypeInfoUnknownType and not TypeTypeInfo && passingParams[i].Type is TypeTypeInfo)
                continue;

            // type specific check
            // only do this check if it's a copy type
            if (funcParams[i].Type.IsAssignableFrom(passingParams[i].Type) && funcParams[i].Modifier == VariableModifier.COPY)
                continue;

            // if none of the conditions pass this will be hit
            return false;
        }

        return true;
    }
}

public class ParameterInfo(TypeInfo type, VariableModifier modifier)
{
    public TypeInfo Type { get; } = type;
    public VariableModifier Modifier { get; } = modifier;

    public override string ToString()
    {
        return Type.TypeName + (Modifier == VariableModifier.COPY ? "" : "*");
    }
}