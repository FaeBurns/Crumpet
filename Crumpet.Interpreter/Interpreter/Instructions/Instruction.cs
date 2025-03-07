using Crumpet.Interpreter.Functions;
using Crumpet.Parser;

namespace Crumpet.Interpreter.Instructions;

public abstract class Instruction
{
    public SourceLocation Location { get; set; }

    public abstract void Execute(InterpreterExecutionContext context);
}