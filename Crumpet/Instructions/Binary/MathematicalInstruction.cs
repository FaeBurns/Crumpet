using System.Diagnostics;
using System.Numerics;
using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Binary;

public class MathematicalInstruction : Instruction
{
    private readonly Operation m_operation;

    public MathematicalInstruction(Operation operation, SourceLocation location) : base(location)
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
        // do concat if either arg is a string
        if ((a.Type == BuiltinTypeInfo.String || b.Type == BuiltinTypeInfo.String) && m_operation == Operation.ADD)
        {
            string stringResult = a.GetValue()!.ToString()! + b.GetValue()!.ToString()!;
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
            else throw new TypeMismatchException(ExceptionConstants.INVALID_TYPE.Format("{NUMBER}", a.Type));
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
            else throw new TypeMismatchException(ExceptionConstants.INVALID_TYPE.Format("{float|int}", $"{a.Type}, {b.Type}"));
        }
        
        context.VariableStack.Push(result);
    }

    private Variable Operate<T>(Variable a, Variable b) where T : INumber<T>
    {
        T aVal = a.GetValue<T>();
        T bVal = b.GetValue<T>();

        T result = m_operation switch
        {
            Operation.MULTIPLY => aVal * bVal,
            Operation.DIVIDE => aVal / bVal,
            Operation.ADD => aVal + bVal,
            Operation.SUBTRACT => aVal - bVal,
            Operation.MODULO => aVal % bVal,
            _ => throw new UnreachableException()
        };

        return Variable.Create(a.Type, result);
    }

    public enum Operation
    {
        MULTIPLY,
        DIVIDE,
        ADD,
        SUBTRACT,
        MODULO,
    }
}