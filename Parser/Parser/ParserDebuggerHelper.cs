using System.Diagnostics;

namespace Parser;

public static class ParserDebuggerHelper<T> where T : Enum
{
    private static HashSet<Type> s_tryingNonTerminals = new HashSet<Type>();
    private static HashSet<Type> s_successNonTerminals = new HashSet<Type>();
    private static HashSet<T> s_terminals = new HashSet<T>();
    private static HashSet<string> s_terminalContent = new HashSet<string>();
    
    public static void SetBreakingNonTerminalsTrying(params IEnumerable<Type> nonTerminals)
    {
        s_tryingNonTerminals = new HashSet<Type>(nonTerminals);
    }
    
    public static void SetBreakingNonTerminalsSuccess(params IEnumerable<Type> nonTerminals)
    {
        s_successNonTerminals = new HashSet<Type>(nonTerminals);
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
        s_tryingNonTerminals.Clear();
        s_successNonTerminals.Clear();
        s_terminals.Clear();
    }

    [Conditional("DEBUG")]
    public static void BreakIfNecessaryTrying(Type nonTerminalType)
    {
        if (s_tryingNonTerminals.Contains(nonTerminalType))
            Debugger.Break();
    }
    
    [Conditional("DEBUG")]
    public static void BreakIfNecessarySuccess(Type nonTerminalType)
    {
        if (s_successNonTerminals.Contains(nonTerminalType))
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