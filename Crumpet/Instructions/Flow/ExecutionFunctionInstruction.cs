using System.Diagnostics;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;

namespace Crumpet.Instructions.Flow;

public class ExecutionFunctionInstruction : Instruction
{
    private readonly string m_functionName;

    public ExecutionFunctionInstruction(string functionName)
    {
        m_functionName = functionName;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Function function = context.FunctionResolver.GetFunction(m_functionName);
        
        // assert that there's enough variables to pop for function
        Debug.Assert(context.VariableStack.Count >= function.Definition.Parameters.Count);
        
        // reverse for to pop in reverse order
        // they're placed on the stack in the correct order but that's not the order they'll get popped off so a reverse is required
        Variable[] args = new Variable[function.Definition.Parameters.Count];
        for (int i = function.Definition.Parameters.Count - 1; i >= 0; i--)
        {
            args[i] = context.VariableStack.Pop();
        }
        
        context.Call(function.CreateInvokableUnit(context, args));
    }
}