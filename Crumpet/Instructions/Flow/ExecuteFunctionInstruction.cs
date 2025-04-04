using System.Diagnostics;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Flow;

public class ExecuteFunctionInstruction : Instruction
{
    private readonly string m_functionName;
    private readonly int m_typeArgumentCount;
    private readonly int m_argumentCount;

    public ExecuteFunctionInstruction(string functionName, int typeArgumentCount, int argumentCount, SourceLocation location) : base(location)
    {
        m_functionName = functionName;
        m_typeArgumentCount = typeArgumentCount;
        m_argumentCount = argumentCount;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        InterpreterDebuggerHelper.BreakOnFunction(m_functionName);
        
        // get arguments and their types
        // then get a function that matches the parameters
        // reverse the order too as they're on the stack in the reverse order to what we need
        IReadOnlyList<TypeInfo> typeArgs = context.VariableStack.Pop(m_typeArgumentCount).Select(v => v.GetValue<TypeInfo>()).Reverse().ToArray();
        IEnumerable<Variable> argumentVariables = context.VariableStack.Peek(m_argumentCount);
        IEnumerable<ParameterInfo> parameterTypes = argumentVariables.Select(v => new ParameterInfo(v.Type, v.Modifier)).Reverse();
        Function function = context.FunctionResolver.GetFunction(m_functionName, parameterTypes, typeArgs);

        switch (function)
        {
            case BuiltInFunction builtInFunction:
                ExecuteBuiltInFunction(context, builtInFunction, typeArgs);
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
        Debug.Assert(context.VariableStack.Count >= function.Parameters.Count);
        
        // reverse for to pop in reverse order
        // they're placed on the stack in evaluation order but that's not the order they'll get popped off so a reverse is required
        Variable[] args = new Variable[function.Parameters.Count];
        for (int i = args.Length - 1; i >= 0; i--)
        {
            args[i] = context.VariableStack.Pop();
        }
        
        // create the unit and call it
        context.Call(function.CreateInvokableUnit(context, args), function.ReturnType);
    }

    private void ExecuteBuiltInFunction(InterpreterExecutionContext context, BuiltInFunction function, IReadOnlyList<TypeInfo> typeArgs)
    {
        // just call it directly - it's up to the function to handle the rest
        function.Invoke(context, typeArgs);
    }
}