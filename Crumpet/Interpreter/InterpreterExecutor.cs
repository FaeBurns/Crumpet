using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter;

public class InterpreterExecutor
{
    private readonly InterpreterExecutionContext m_context;
    
    internal InterpreterExecutor(InterpreterExecutionContext context)
    {
        m_context = context;
    }
    
    public Variable StepUntilComplete()
    {
        // return null if invalid
        if (m_context.CurrentUnit == null)
            return null!;

        // execute all instructions
        while (StepSingleInstruction())
        {
        }

        // return last returned value or default of 0
        return m_context.ExecutionResult ?? BuiltinTypeInfo.Int.CreateVariable();
    }
    
    private bool StepSingleInstruction()
    {
        if (m_context.CurrentUnit == null)
            return false;

        // get the next instruction
        // null case means that there were no units left to get instructions from and the program is complete
        Instruction? instruction = m_context.StepNextInstruction();
        if (instruction == null)
            return false;

        InterpreterDebuggerHelper.BreakAtLocation(instruction.Location);
        
        // conditional compilation here fucks with user exceptions but helps with debugging ig???
        #if DEBUG && FALSE
            instruction.Execute(m_context);
        #else
            
        try
        {
            instruction.Execute(m_context);
        }
        catch (UncaughtRuntimeExceptionException e)
        {
            throw new InterpreterException(instruction.Location, ExceptionConstants.UNCAUGHT_RUNTIME_EXCEPTION, e);
        }
        catch (Exception e)
        {
            // if it's not coming from a throw then throw it in user context
            try
            {
                // catch again as this can throw
                m_context.Throw(e.ToString());
            }
            catch (UncaughtRuntimeExceptionException r)
            {
                throw new InterpreterException(instruction.Location, ExceptionConstants.UNCAUGHT_RUNTIME_EXCEPTION, r);
            }
        }
        #endif

        return true;
    }
}