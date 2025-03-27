using System.Diagnostics;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Details;

public class RestoreStackItemCountInstruction : Instruction
{
    private readonly StackItemCounter m_counter;

    public RestoreStackItemCountInstruction(StackItemCounter counter, SourceLocation location) : base(location)
    {
        m_counter = counter;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        // pop off until there's the same amount in the counter as the stack
        while (context.VariableStack.Count > m_counter.Count)
        {
            context.VariableStack.Pop();
        }
        
        Debug.Assert(context.VariableStack.Count == m_counter.Count);
    }
}