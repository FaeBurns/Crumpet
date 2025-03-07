using Crumpet.Interpreter.Instructions;

namespace Crumpet.Interpreter.Functions;

public class ExecutableUnit
{
    public ExecutableUnit(InterpreterExecutionContext context, IEnumerable<Instruction> instructions, FunctionDefinition functionDefinition)
    {
        Scope = new Scope(context.CurrentScope);
        Instructions = instructions.ToArray();
        FunctionDefinition = functionDefinition;
    }

    public IReadOnlyList<Instruction> Instructions { get; }
    public Scope Scope { get; }
    public FunctionDefinition FunctionDefinition { get; set; }
}