using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions;

public class PushNamedVariableInstruction : Instruction
{
    private readonly string m_name;

    public PushNamedVariableInstruction(string name, SourceLocation location) : base(location)
    {
        m_name = name;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable var = context.CurrentScope.GetVariable(m_name);
        var.SourceName = m_name;
        context.VariableStack.Push(var);
    }
}