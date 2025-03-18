using Crumpet.Interpreter.Functions;
using Parser;
using Shared;

namespace Crumpet.Interpreter.Instructions;

public abstract class Instruction
{
    public abstract void Execute(InterpreterExecutionContext context);
}