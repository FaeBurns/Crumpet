using System.Diagnostics;
using System.Numerics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Binary;

public class MathematicalInstruction : Instruction
{
    private readonly Operation m_operation;

    public MathematicalInstruction(Operation operation)
    {
        m_operation = operation;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        // pop in reverse order
        Variable b = context.VariableStack.Pop();
        Variable a = context.VariableStack.Pop();
        
        // string concat
        // kinda stinky putting it here but it works?
        if (a.Type == b.Type && a.Type == BuiltinTypeInfo.String && m_operation == Operation.ADD)
        {
            string stringResult = a.GetValue<string>() + b.GetValue<string>();
            context.VariableStack.Push(BuiltinTypeInfo.String, stringResult);
            return;
        }
        
        Variable result;
        
        // both values are the same type
        if (a.Type == b.Type)
        {
            if (a.Type == BuiltinTypeInfo.Int)
            {
                result = Operate<int>(a, b);
            }
            else if (a.Type == BuiltinTypeInfo.Float)
            {
                result = Operate<float>(a, b);
            }
            
            // not a valid type
            else throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format("{NUMBER}", a.Type));
        }
        // both values are not the same type
        else
        {
            // both orders
            if (a.Type == BuiltinTypeInfo.Int && b.Type == BuiltinTypeInfo.Float)
            {
                result = Operate<float>(a, b);
            }
            else if (a.Type == BuiltinTypeInfo.Float && b.Type == BuiltinTypeInfo.Int)
            {
                result = Operate<float>(a, b);
            }
            
            // both are not valid types
            else throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format("{float|int}", $"{a.Type}, {b.Type}"));
        }
        
        context.VariableStack.Push(result);
    }

    private Variable Operate<T>(Variable a, Variable b) where T : INumber<T>
    {
        T aVal = a.GetValue<T>();
        T bVal = b.GetValue<T>();

        T result;

        switch (m_operation)
        {
            case Operation.MULTIPLY:
                result = aVal * bVal;
                break;
            case Operation.DIVIDE:
                result = aVal / bVal;
                break;
            case Operation.ADD:
                result = aVal + bVal;
                break;
            case Operation.SUBTRACT:
                result = aVal - bVal;
                break;
            default:
                throw new UnreachableException();
        }

        return Variable.Create(a.Type, result);
    }

    public enum Operation
    {
        MULTIPLY,
        DIVIDE,
        ADD,
        SUBTRACT,
    }
}