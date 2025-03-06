using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Parser;

namespace Crumpet.Interpreter.Functions;

public class ExecutableUnit
{
    public ExecutableUnit(ExecutionContext context, IEnumerable<IInstruction> instructions, SourceLocation location)
    {
        Scope = new Scope(context.CurrentScope);
        Instructions = instructions.ToArray();
        SourceLocation = location;
    }
    
    public IReadOnlyList<IInstruction> Instructions { get; }
    public Scope Scope { get; }
    public SourceLocation SourceLocation { get; }
}