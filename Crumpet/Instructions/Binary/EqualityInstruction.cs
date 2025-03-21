using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Instructions.Binary;

public class EqualityInstruction : Instruction
{
    private readonly bool m_invert;

    public EqualityInstruction(bool invert)
    {
        m_invert = invert;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable a = context.VariableStack.Pop();
        Variable b = context.VariableStack.Pop();

        // use .Equals so a value comparison is done if necessary
        bool result = a.Value.Equals(b.Value);
        if (m_invert)
            result = !result;

        context.VariableStack.Push(BuiltinTypeInfo.Bool, result);
    }
}