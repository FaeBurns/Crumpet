using System.Diagnostics;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Instructions.Binary;

public class LogicalBooleanInstruction : Instruction
{
    private readonly Operation m_operation;

    public LogicalBooleanInstruction(Operation operation)
    {
        m_operation = operation;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        bool a = context.VariableStack.Pop().GetValue<bool>();
        bool b = context.VariableStack.Pop().GetValue<bool>();

        bool result = m_operation switch
        {
            // == and != need to be handled more generically. Only these can be assumed to be bools.
            Operation.AND => a && b,
            Operation.OR => a || b,
            _ => throw new UnreachableException(),
        };

        context.VariableStack.Push(Variable.Create(BuiltinTypeInfo.Bool, result));
    }

    public enum Operation
    {
        AND,
        OR,
        XOR,
    }
}