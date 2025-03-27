using System.Diagnostics;
using Shared;

namespace Crumpet.Interpreter;

public static class InterpreterDebuggerHelper
{
    private static readonly HashSet<string> s_functions = new HashSet<string>();
    private static readonly HashSet<LocationRecord> s_locations = new HashSet<LocationRecord>();
    
    public static void RegisterFunction(string name)
    {
        s_functions.Add(name);
    }

    public static void RegisterLocation(int line, int column)
    {
        s_locations.Add(new LocationRecord(line, column));
    }

    public static void Clear()
    {
        s_functions.Clear();
        s_locations.Clear();
    }
    
    [Conditional("DEBUG")]
    internal static void BreakOnFunction(string name)
    {
        if (s_functions.Contains(name))
            Debugger.Break();
    }

    [Conditional("DEBUG")]
    internal static void BreakAtLocation(SourceLocation location)
    {
        if (s_locations.Contains(new LocationRecord(location.StartLine + 1, location.StartColumn + 1)))
            Debugger.Break();
    }
    
    private record LocationRecord(int Line, int Column); 
}