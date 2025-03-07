using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Parser;

namespace Crumpet.Interpreter.Functions;

public class InterpreterExecutionContext
{
    private readonly Scope m_rootScope = new Scope(null);
    private readonly Stack<UnitExecutionContext> m_executionStack = new Stack<UnitExecutionContext>();

    public UnitExecutionContext? CurrentUnit => m_executionStack.Any() ? m_executionStack.Peek() : null;
    public Scope CurrentScope => CurrentUnit?.Unit.Scope ?? m_rootScope;
    public Variable? LatestReturnValue { get; private set; }
    public VariableStack VariableStack { get; } = new VariableStack();

    public void Call(ExecutableUnit unit)
    {
        m_executionStack.Push(new UnitExecutionContext(unit));
    }

    public void Return(Variable? value)
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException();

        m_executionStack.Pop();
        LatestReturnValue = value;
    }
}

public class UnitExecutionContext(ExecutableUnit unit)
{
    public ExecutableUnit Unit { get; } = unit;
    public int InstructionPointer { get; private set; }
    public Instruction CurrentInstruction => Unit.Instructions[InstructionPointer];
    public bool IsComplete => InstructionPointer >= Unit.Instructions.Count;

    /// <summary>
    /// Gets the next instruction and advances the <see cref="InstructionPointer"/>
    /// </summary>
    /// <returns></returns>
    public Instruction StepNextInstruction()
    {
        return Unit.Instructions[InstructionPointer++];
    }

    public SourceLocation UnitLocation => Unit.FunctionDefinition.SourceLocation;
}