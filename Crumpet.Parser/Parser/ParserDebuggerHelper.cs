using System.Collections;
using System.Diagnostics;

namespace Crumpet.Interpreter.Parser;

public static class ParserDebuggerHelper<T> where T : Enum
{
    private static HashSet<Type> s_nonTerminals = new HashSet<Type>();
    private static HashSet<T> s_terminals = new HashSet<T>();
    private static HashSet<string> s_terminalContent = new HashSet<string>();
    
    public static void SetBreakingNonTerminals(params IEnumerable<Type> nonTerminals)
    {
        s_nonTerminals = new HashSet<Type>(nonTerminals);
    }

    public static void SetBreakingTerminals(params IEnumerable<T> terminals)
    {
        s_terminals = new HashSet<T>(terminals);
    }

    public static void SetBreakingTerminalContent(params IEnumerable<string> terminalContent)
    {
        s_terminalContent = new HashSet<string>(terminalContent);
    }

    public static void Clear()
    {
        s_nonTerminals.Clear();
        s_terminals.Clear();
    }

    [Conditional("DEBUG")]
    public static void BreakIfNecessary(Type nonTerminalType)
    {
        if (s_nonTerminals.Contains(nonTerminalType))
            Debugger.Break();
    }

    [Conditional("DEBUG")]
    public static void BreakIfNecessary(T terminal)
    {
        if (s_terminals.Contains(terminal))
            Debugger.Break();
    }

    [Conditional("DEBUG")]
    public static void BreakIfNecessary(string terminalContent)
    {
        if (s_terminalContent.Contains(terminalContent))
            Debugger.Break();
    }
}