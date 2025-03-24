using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Instructions;

public class AssertReturnTypeInstruction : Instruction
{
    private readonly string m_typeName;
    private readonly VariableModifier m_modifier;

    public AssertReturnTypeInstruction(string typeName, VariableModifier modifier, SourceLocation location) : base(location)
    {
        m_typeName = typeName;
        m_modifier = modifier;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        TypeInfo? type = context.TypeResolver.ResolveType(m_typeName);
        if (type is null)
            throw new TypeMismatchException(ExceptionConstants.UNKOWN_TYPE.Format(m_typeName));

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