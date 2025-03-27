using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class LoopBreakLabel : Instruction
{
    public LoopBreakLabel(SourceLocation location) : base(location)
    {
    }
    public override void Execute(InterpreterExecutionContext context)
    {
    }
}