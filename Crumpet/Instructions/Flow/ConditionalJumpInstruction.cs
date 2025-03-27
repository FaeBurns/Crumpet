using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Flow;

public class ConditionalJumpInstruction : Instruction
{
    private readonly Guid? m_trueLabel;
    private readonly Guid? m_falseLabel;

    public ConditionalJumpInstruction(Guid? trueLabel, Guid? falseLabel, SourceLocation location) : base(location)
    {
        m_trueLabel = trueLabel;
        m_falseLabel = falseLabel;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable condition = context.VariableStack.Pop();
        if (condition.GetValue<bool>())
        {
            if (m_trueLabel.HasValue)
                context.Jump(m_trueLabel.Value);
        }
        else
        {
            if (m_falseLabel.HasValue)
                context.Jump(m_falseLabel.Value);
        }
    }
}