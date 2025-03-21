using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions;

public class ExecuteUnitInstruction : Instruction
{
    private readonly Instruction[] m_instructions;
    public bool AcceptsReturn { get; set; }
    
    public ExecuteUnitInstruction(IEnumerable<Instruction> instructions, SourceLocation location) : base(location)
    {
        m_instructions = instructions.ToArray();
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, Location);
        unit.AcceptsReturn = AcceptsReturn;
        context.Call(unit);
    }
}