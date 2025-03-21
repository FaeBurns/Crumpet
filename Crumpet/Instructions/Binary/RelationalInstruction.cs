using System.Diagnostics;
using System.Numerics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Binary;

public class RelationalInstruction : Instruction
{
    private readonly Operation m_operation;

    public RelationalInstruction(Operation operation, SourceLocation location) : base(location)
    {
        m_operation = operation;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        // pop in reverse order
        Variable b = context.VariableStack.Pop();
        Variable a = context.VariableStack.Pop();

        bool result = Operate(context, a, b);
        
        context.VariableStack.Push(BuiltinTypeInfo.Bool, result);
    }

    private bool Operate(InterpreterExecutionContext context, Variable a, Variable b)
    {
        if (a.Type != b.Type)
        {
            if ((a.AssertType<int>() && b.AssertType<float>()) || (a.AssertType<float>() && b.AssertType<float>()))
            {
                return Operate<float>(a, b);
            }
            throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format("{float|int}", $"{a.Type}, {b.Type}"));
        }
        else if (a.Type == b.Type)
        {
            if (a.AssertType<int>())
            {
                return Operate<int>(a, b);
            }
            else if (a.AssertType<float>())
            {
                return Operate<float>(a, b);
            }
        }

        throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format("{NUMBER}", a.Type));
    }

    private bool Operate<T>(Variable a, Variable b) where T : INumber<T>
    {
        T aVal = a.GetValue<T>();
        T bVal = b.GetValue<T>();

        return m_operation switch
        {
            Operation.LESS => aVal < bVal,
            Operation.LESS_OR_EQUAL => aVal <= bVal,
            Operation.GREATER => aVal > bVal,
            Operation.GREATER_OR_EQUAL => aVal >= bVal,
            _ => throw new UnreachableException(),
        };
    }

    public enum Operation
    {
        LESS,
        LESS_OR_EQUAL,
        GREATER,
        GREATER_OR_EQUAL,
    }
}