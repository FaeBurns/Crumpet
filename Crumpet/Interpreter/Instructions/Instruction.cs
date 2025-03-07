using Crumpet.Interpreter.Functions;
using Parser;
using Shared;

namespace Crumpet.Interpreter.Instructions;

public abstract class Instruction
{
    public SourceLocation Location { get; set; }

    public abstract void Execute(InterpreterExecutionContext context);
}