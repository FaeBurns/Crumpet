using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Instructions;

public class AssertReturnTypeInstruction : Instruction
{
    private readonly VariableModifier m_modifier;

    public AssertReturnTypeInstruction(VariableModifier modifier, SourceLocation location) : base(location)
    {
        m_modifier = modifier;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        TypeInfo type = context.VariableStack.Pop().GetValue<TypeInfo>();

        if (context.VariableStack.Count == 0)
            throw new RuntimeException(RuntimeExceptionNames.RETURN, ExceptionConstants.MISSING_RETURN_STATEMENT.Format(type.TypeName));
            
        Variable returnValue = context.VariableStack.Peek();

        // allow null through
        if (returnValue.Type is NullTypeInfo && m_modifier == VariableModifier.POINTER)
            return;
        
        if (m_modifier != returnValue.Modifier)
            throw new InvalidOperationException(ExceptionConstants.MODIFIER_MISMATCH.Format(m_modifier, returnValue.Modifier));

        switch (m_modifier)
        {
            case VariableModifier.COPY:
                if (!returnValue.Type.IsAssignableTo(type))
                    throw new ArgumentException(ExceptionConstants.INVALID_RETURN_TYPE.Format(type, returnValue.Type));
                break;
            case VariableModifier.POINTER:
                if (returnValue.Type != type)
                    throw new ArgumentException(ExceptionConstants.INVALID_RETURN_TYPE.Format(type, returnValue.Type));
                break;
            default:
                throw new UnreachableException();
        }
    }
}