namespace Crumpet.Interpreter.Functions;

public class ExecutionContext
{
    private readonly Scope m_rootScope = new Scope(null);
    private readonly Stack<ExecutableUnit> m_executionStack = new Stack<ExecutableUnit>();
    
    public int InstructionCounter { get; }
    public ExecutableUnit? CurrentUnit => m_executionStack.Any() ? m_executionStack.Peek() : null;
    public Scope CurrentScope => CurrentUnit?.Scope ?? m_rootScope;
}