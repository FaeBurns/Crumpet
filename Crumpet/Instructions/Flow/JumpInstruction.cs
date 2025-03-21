using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class JumpInstruction : Instruction
{
    public Guid Target { get; }

    public JumpInstruction(Guid target, SourceLocation location) : base(location)
    {
        Target = target;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        context.Jump(Target);
    }
}