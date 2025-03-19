using Shared;

namespace Crumpet.Interpreter.Functions;

public class FunctionResolver
{
    private readonly Dictionary<string, Function> m_functions;
    
    public FunctionResolver(IEnumerable<Function> functions)
    {
        m_functions = functions.ToDictionary(f => f.Name);
    }

    public Function GetFunction(string functionName)
    {
        return m_functions.GetValueOrDefault(functionName)
               ?? throw new KeyNotFoundException(ExceptionConstants.FUNCTION_NOT_FOUND.Format(functionName));
    }
}