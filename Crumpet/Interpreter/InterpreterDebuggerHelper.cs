using System.Diagnostics;

namespace Crumpet.Interpreter;

public static class InterpreterDebuggerHelper
{
    private static readonly HashSet<string> s_functions = new HashSet<string>();
    
    public static void RegisterFunction(string name)
    {
        s_functions.Add(name);
    }

    public static void Clear()
    {
        s_functions.Clear();
    }
    
    [Conditional("DEBUG")]
    internal static void BreakOnFunction(string name)
    {
        if (s_functions.Contains(name))
            Debugger.Break();
    }
}