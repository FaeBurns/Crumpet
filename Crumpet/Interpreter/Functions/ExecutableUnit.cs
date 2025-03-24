using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class ExecutableUnit
{
    public ExecutableUnit(InterpreterExecutionContext context, IEnumerable<Instruction> instructions, SourceLocation sourceLocation, bool blocksScope)
    {
        SourceLocation = sourceLocation;
        
        BlocksScope = blocksScope;
        if (BlocksScope)
            Scope = new Scope(null);
        else 
            Scope = new Scope(context.CurrentScope);
        
        Instructions = instructions.ToArray();
    }

    public IReadOnlyList<Instruction> Instructions { get; }
    public Scope Scope { get; }
    public SourceLocation SourceLocation { get; }
    public bool BlocksScope { get; }
}