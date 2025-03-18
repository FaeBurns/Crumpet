using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions.Flow;

public class JumpInstruction : Instruction
{
    public Guid Target { get; }

    public JumpInstruction(Guid target)
    {
        Target = target;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        context.Jump(Target);
    }
}