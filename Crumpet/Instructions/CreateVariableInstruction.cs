using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Instructions;

public class CreateVariableInstruction : Instruction
{
    private readonly string m_name;
    private readonly VariableModifier m_modifier;
    private readonly bool m_isArray;

    public CreateVariableInstruction(string name, VariableModifier modifier, bool isArray, SourceLocation location) : base(location)
    {
        m_name = name;
        m_modifier = modifier;
        m_isArray = isArray;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        TypeInfo type = context.VariableStack.Pop().GetValue<TypeInfo>();

        // if it's an array, encapsulate the type in an array
        if (m_isArray)
            type = new ArrayTypeInfo(type);

        context.CurrentScope.Create(new VariableInfo(m_name, type, m_modifier));
    }
}