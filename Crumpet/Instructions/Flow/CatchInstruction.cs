using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Flow;

public class CatchInstruction : Instruction
{
    private readonly string? m_messageVariableName;

    public CatchInstruction(string? messageVariableName, SourceLocation location) : base(location)
    {
        m_messageVariableName = messageVariableName;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        if (m_messageVariableName != null)
        {
            Variable variable = context.VariableStack.Pop();
            context.CurrentScope.Add(m_messageVariableName, variable);
        }
    }
}