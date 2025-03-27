using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Binary;

public class EqualityInstruction : Instruction
{
    private readonly bool m_invert;

    public EqualityInstruction(bool invert, SourceLocation location) : base(location)
    {
        m_invert = invert;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable a = context.VariableStack.Pop();
        Variable b = context.VariableStack.Pop();

        // use .Equals so a value comparison is done if necessary
        // bool result = a.Value.Equals(b.Value);
        
        // compare via dynamic to allow cross-type comparisons. e.g. float(3) == int(3)
        dynamic? aVal = a.GetValue();
        dynamic? bVal = b.GetValue();
        bool result = aVal == bVal;
        if (m_invert)
            result = !result;

        context.VariableStack.Push(BuiltinTypeInfo.Bool, result);
    }
}