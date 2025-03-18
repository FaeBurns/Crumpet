using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions;

public class ExecuteUnitInstruction : Instruction
{
    private readonly SourceLocation m_location;
    private readonly Instruction[] m_instructions;
    public bool AcceptsReturn { get; set; }
    
    public ExecuteUnitInstruction(SourceLocation location, IEnumerable<Instruction> instructions)
    {
        m_location = location;
        m_instructions = instructions.ToArray();
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, m_location);
        unit.AcceptsReturn = AcceptsReturn;
        context.Call(unit);
    }
}