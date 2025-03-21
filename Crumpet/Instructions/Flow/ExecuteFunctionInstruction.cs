using System.Diagnostics;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Instructions.Flow;

public class ExecuteFunctionInstruction : Instruction
{
    private readonly string m_functionName;
    private readonly int m_argumentCount;

    public ExecuteFunctionInstruction(string functionName, int argumentCount)
    {
        m_functionName = functionName;
        m_argumentCount = argumentCount;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        // get arguments and their types
        // then get a function that matches the parameters
        // reverse the order too as they're on the stack in the reverse order to what we desire
        IEnumerable<Variable> argumentVariables = context.VariableStack.Peek(m_argumentCount);
        IEnumerable<TypeInfo> parameterTypes = argumentVariables.Select(v => v.Type).Reverse();
        Function function = context.FunctionResolver.GetFunction(m_functionName, parameterTypes);

        switch (function)
        {
            case BuiltInFunction builtInFunction:
                ExecuteBuiltInFunction(context, builtInFunction);
                break;
            case UserFunction userFunction:
                ExecuteUserFunction(context, userFunction);
                break;
            default:
                throw new UnreachableException();
        }
    }

    private void ExecuteUserFunction(InterpreterExecutionContext context, UserFunction function)
    {
        // assert that there's enough variables to pop for function
        Debug.Assert(context.VariableStack.Count >= function.Definition.Parameters.Count);
        
        // reverse for to pop in reverse order
        // they're placed on the stack in evaluation order but that's not the order they'll get popped off so a reverse is required
        Variable[] args = new Variable[function.Definition.Parameters.Count];
        for (int i = function.Definition.Parameters.Count - 1; i >= 0; i--)
        {
            args[i] = context.VariableStack.Pop();
        }
        
        context.Call(function.CreateInvokableUnit(context, args));
    }

    private void ExecuteBuiltInFunction(InterpreterExecutionContext context, BuiltInFunction function)
    {
        function.Invoke(context);
    }
}