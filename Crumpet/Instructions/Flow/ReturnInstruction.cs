using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Flow;

public class ReturnInstruction : Instruction
{
    private readonly bool m_returnsValue;

    public ReturnInstruction(bool returnsValue, SourceLocation location) : base(location)
    {
        m_returnsValue = returnsValue;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        if (m_returnsValue)
        {
            Variable var = context.VariableStack.Pop();
            context.Return(var);
        }
        else
        {
            context.Return(null);
        }
    }
}