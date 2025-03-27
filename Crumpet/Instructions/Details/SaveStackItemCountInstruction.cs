using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Details;

public class SaveStackItemCountInstruction : Instruction
{
    private readonly StackItemCounter m_counter;

    public SaveStackItemCountInstruction(StackItemCounter counter, SourceLocation location) : base(location)
    {
        m_counter = counter;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        m_counter.Count = context.VariableStack.Count;
    }
}

public class StackItemCounter
{
    public StackItemCounter()
    {
    }
    
    public int Count { get; set; }
}