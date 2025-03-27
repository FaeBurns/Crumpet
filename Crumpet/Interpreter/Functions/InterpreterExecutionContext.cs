using Crumpet.Exceptions;
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
    private readonly string m_source;
    private readonly Scope m_rootScope = new Scope(null);
    private readonly Stack<UnitExecutionContext> m_executionStack = new Stack<UnitExecutionContext>();

    public UnitExecutionContext? CurrentUnit => m_executionStack.Any() ? m_executionStack.Peek() : null;
    public Scope CurrentScope => CurrentUnit?.Unit.Scope ?? m_rootScope;
    public ValueSearcher ValueSearcher => new ValueSearcher(CurrentScope);
    public VariableStack VariableStack { get; } = new VariableStack();
    public TypeResolver TypeResolver { get; }
    public FunctionResolver FunctionResolver { get; }
    public Variable? ExecutionResult { get; set; }

    public InterpreterExecutionContext(TypeResolver typeResolver, FunctionResolver functionResolver, Stream inputStream, Stream outputStream, string source)
    {
        m_inputStream = inputStream;
        m_outputStream = outputStream;
        m_source = source;
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
                    // point to the one before - it will be incremented before access
                    searchingUnit.InstructionPointer = searchingUnit.Unit.Instructions.IndexOf(instruction);
                    return;
                }
            }
            
            // pop from execution to search in the unit above 
            m_executionStack.Pop();
            searchingUnit = m_executionStack.Peek();
        }
    }

    public void Call(ExecutableUnit unit, TypeInfo? expectedTopmostValueType = null)
    {
        m_executionStack.Push(new UnitExecutionContext(unit, expectedTopmostValueType ?? new AnyTypeInfo()));
    }

    public void Return(Variable? value)
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);
        
        // push the return value if its not a void function
        if (value is not null)
            VariableStack.Push(value);
        
        // record to execution result
        // this will not occur during a built in function but those should never be an entry point so it's ok?
        ExecutionResult = value;

        // always pop at least one
        JumpToInstructionOfType<ReturnLabelInstruction>(false);

        // return type check
        if (value is not null)
        {
            if (!value.Type.IsAssignableTo(CurrentUnit.ExpectedReturnType))
                throw new TypeMismatchException(CurrentUnit.ExpectedReturnType, value.Type);
        }

        // pop another one off
        // this one was the one accepting the return
        // m_executionStack.Pop();
        // we're now outside of that function
        
        // actually don't pop one off here as the pop in the loop will do that for us. If that was using the peek search method then yes a pop should be used here
    }

    public void Continue()
    {
        JumpToInstructionOfType<LoopContinueLabel>(true);
    }

    public void Break()
    {
        JumpToInstructionOfType<LoopBreakLabel>(true);
    }
    
    public void Exit(int exitCode)
    {
        ExecutionResult = Variable.Create(BuiltinTypeInfo.Int, exitCode);
        m_executionStack.Clear();
    }
    
    /// <summary>
    /// Throws an exception inside user code. 
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <exception cref="UncaughtRuntimeExceptionException">The exception was not caught by user code.</exception>
    public void Throw(string message)
    {
        try
        {
            // first pop something off
            // then try and search for the catch
            // this stops exceptions thrown inside a catch statement from being caught by the same statement again
            m_executionStack.Pop();
            JumpToInstructionOfType<CatchInstruction>(false);
            VariableStack.Push(Variable.Create(BuiltinTypeInfo.String, message));
        }
        catch
        {
            throw new UncaughtRuntimeExceptionException(message);
        }
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

    private void JumpToInstructionOfType<T>(bool blockedByReturn) where T : Instruction
    {
        // invalid if no unit is currently active
        if (CurrentUnit == null)
            throw new InvalidOperationException(ExceptionConstants.NO_EXECUTING_UNIT);
        
        UnitExecutionContext searchingUnit = CurrentUnit;

        while (m_executionStack.Any())
        {
            foreach (Instruction instruction in searchingUnit.Unit.Instructions)
            {
                if (instruction is ReturnLabelInstruction && blockedByReturn)
                    break;
                
                if (instruction is T)
                {
                    // set instruction pointer to point to current instruction
                    // actually point to the one beforehand
                    searchingUnit.InstructionPointer = searchingUnit.Unit.Instructions.IndexOf(instruction);
                    return;
                }
            }
            
            // pop from execution to search in the unit above 
            m_executionStack.Pop();
            // get the searching unit from the peek as we want the unit with the target instruction to be on the stack
            searchingUnit = CurrentUnit;
        }

        throw new KeyNotFoundException(ExceptionConstants.COULD_NOT_FIND_INSTRUCTION_TO_JUMP_TO.Format(typeof(T)));
    }

    public ReadOnlySpan<char> GetSourceFromLocation(SourceLocation location)
    {
        return m_source.AsSpan().Slice(location.StartOffset, location.LengthOffset);
    }
}