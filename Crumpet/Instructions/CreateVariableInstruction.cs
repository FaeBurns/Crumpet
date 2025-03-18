using Crumpet.Exceptions;
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
    private readonly string m_typeName;
    private readonly VariableModifier m_modifier;

    public CreateVariableInstruction(string name, string typeName, VariableModifier modifier)
    {
        m_name = name;
        m_typeName = typeName;
        m_modifier = modifier;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        TypeInfo? type = context.TypeResolver.ResolveType(m_typeName);
        if (type is null)
            throw new InterpreterException(context, ExceptionConstants.UNKOWN_TYPE);
            
        context.CurrentScope.Create(new VariableInfo(m_name, type, m_modifier));
    }
}