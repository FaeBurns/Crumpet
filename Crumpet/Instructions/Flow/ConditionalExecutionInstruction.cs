using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Flow;

public class ConditionalExecutionInstruction : Instruction
{
    private readonly SourceLocation m_location;
    private readonly Instruction[] m_trueInstructions;
    private readonly Instruction[] m_falseInstructions;

    public ConditionalExecutionInstruction(SourceLocation location, IEnumerable<Instruction> trueInstructions, IEnumerable<Instruction> falseInstructions)
    {
        m_location = location;
        m_trueInstructions = trueInstructions.ToArray();
        m_falseInstructions = falseInstructions.ToArray();
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable condition = context.VariableStack.Pop();
        if (condition.GetValue<bool>())
        {
            ExecuteInstructionSet(context, m_trueInstructions);
        }
        else
        {
            // do not execute instruction set if there are no instructions in the false case
            if (m_falseInstructions.Length > 0)
                ExecuteInstructionSet(context, m_falseInstructions);
        }
    }

    private void ExecuteInstructionSet(InterpreterExecutionContext context, Instruction[] instructions)
    {
        ExecutableUnit unit = new ExecutableUnit(context, instructions, m_location);
        context.Call(unit);
    }
}