using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class BreakInstruction : Instruction
{
    public BreakInstruction(SourceLocation location) : base(location)
    {
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        context.Break();
    }
}