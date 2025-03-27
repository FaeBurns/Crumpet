using System.Diagnostics;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Instructions;

public class PushConstantInstruction : Instruction
{
    private readonly TypeInfo m_type;
    private readonly object? m_value;
    private readonly VariableModifier m_modifier;

    public PushConstantInstruction(TypeInfo type, object? value, VariableModifier modifier, SourceLocation location) : base(location)
    {
        m_type = type;
        m_value = value;
        m_modifier = modifier;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        switch (m_modifier)
        {
            case VariableModifier.COPY:
                context.VariableStack.Push(Variable.Create(m_type, m_value));
                break;
            case VariableModifier.POINTER:
                context.VariableStack.Push(Variable.CreatePointer(m_type, Variable.Create(m_type, m_value)));
                break;
            default:
                throw new UnreachableException();
        }
    }
}