using Crumpet.Exceptions;
using Crumpet.Instructions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class InterpreterExecutionContext
{
    private readonly Stream m_inputStream;
    private readonly Stream m_outputStream;
    private readonly Scope m_rootScope = new Scope(null);
    private readonly Stack<UnitExecutionContext> m_executionStack = new Stack<UnitExecutionContext>();

    public UnitExecutionContext? CurrentUnit => m_executionStack.Any() ? m_executionStack.Peek() : null;
    public Scope CurrentScope => CurrentUnit?.Unit.Scope ?? m_rootScope;
    public ValueSearcher ValueSearcher => new ValueSearcher(CurrentScope);
    public Variable? LatestReturnValue { get; private set; }
    public VariableStack VariableStack { get; } = new VariableStack();
    public TypeResolver TypeResolver { get; }
    public FunctionResolver FunctionResolver { get; }

    public InterpreterExecutionContext(TypeResolver typeResolver, FunctionResolver functionResolver, Stream inputStream, Stream outputStream)
    {
        m_inputStream = inputStream;
        m_outputStream = outputStream;
        TypeResolver = typeResolver;
        FunctionResolver = functionResolver;
    }

    public void Jump(Guid target)
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);
        
        UnitExecutionContext searchingUnit = CurrentUnit;

        while (m_executionStack.Any())
        {
            foreach (Instruction instruction in searchingUnit.Unit.Instructions)
            {
                if (instruction is LabelInstruction label && label.ID == target)
                {
                    // set instruction pointer to point to current instruction
                    searchingUnit.InstructionPointer = searchingUnit.Unit.Instructions.IndexOf(instruction);
                    return;
                }
            }
            
            // pop from execution to search in the unit above 
            m_executionStack.Pop();
        }
    }

    public void Call(ExecutableUnit unit)
    {
        m_executionStack.Push(new UnitExecutionContext(unit));
    }

    public void Return(Variable? value)
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);

        LatestReturnValue = value;

        UnitExecutionContext lastUnit = CurrentUnit;
        while(!lastUnit.Unit.AcceptsReturn)
        {
            // if the last unit does not accept return, pop another one off
            // we're trying to get to the topmost unit that accepts return statements
            lastUnit = m_executionStack.Pop();
        }

        // pop another one off
        // this one was the one accepting the return
        m_executionStack.Pop();
        // we're now outside of that function
    }

    public void Continue()
    {
        JumpToInstructionOfType<LoopContinueLabel>();
    }

    public void Break()
    {
        JumpToInstructionOfType<LoopBreakLabel>();
    }
    
    public void Exit(int exitCode)
    {
        LatestReturnValue = Variable.Create(BuiltinTypeInfo.Int, exitCode);
        m_executionStack.Clear();
    }

    public Instruction? StepNextInstruction()
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);
        
        if (CurrentUnit.IsComplete)
            m_executionStack.Pop();
        
        // then check again?
        // that may have been the last unit
        if (CurrentUnit == null || CurrentUnit.IsComplete)
            return null;
        
        Instruction instruction = CurrentUnit.StepNextInstruction();

        return instruction;
    }

    public void WriteToOutputStream(string output)
    {
        // do not wrap in using as otherwise the streamwriter will dispose the inner stream
        StreamWriter sw = new StreamWriter(m_outputStream);
        sw.Write(output);
        sw.Flush();
    }

    public string ReadInputStreamLine(bool blockUntilInput)
    {
        // do not wrap in using as otherwise the streamreader will dispose the inner stream
        UnbufferedStreamReader sr = new UnbufferedStreamReader(m_inputStream);

        if (!blockUntilInput)
            return sr.ReadLine()?.TrimEnd() ?? string.Empty;

        while (true)
        {
            string? line = sr.ReadLine();
            if (line != null)
                return line.TrimEnd();
        }
    }

    private void JumpToInstructionOfType<T>() where T : Instruction
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);
        
        UnitExecutionContext searchingUnit = CurrentUnit;

        while (m_executionStack.Any())
        {
            foreach (Instruction instruction in searchingUnit.Unit.Instructions)
            {
                if (instruction is T)
                {
                    // set instruction pointer to point to current instruction
                    searchingUnit.InstructionPointer = searchingUnit.Unit.Instructions.IndexOf(instruction);
                    return;
                }
            }
            
            // pop from execution to search in the unit above 
            m_executionStack.Pop();
        }
    }
}

public class UnitExecutionContext(ExecutableUnit unit)
{
    public ExecutableUnit Unit { get; } = unit;
    public int InstructionPointer { get; set; }
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

    public SourceLocation UnitLocation => Unit.SourceLocation;
}