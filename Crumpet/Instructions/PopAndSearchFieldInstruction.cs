using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Instructions;

public class PopAndSearchFieldInstruction : Instruction
{
    private readonly string m_field;

    public PopAndSearchFieldInstruction(string field, SourceLocation location) : base(location)
    {
        m_field = field;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable target = context.VariableStack.Pop();

        if (target.GetValue() == null)
            throw new NullReferenceException();

        if (target.Type is not UserObjectTypeInfo)
            throw new TypeMismatchException(new AnyTypeInfo(), target.Type);

        // requires copy here - all values must be dereferenced beforehand
        if (target.Modifier is VariableModifier.POINTER)
            throw new InvalidOperationException(ExceptionConstants.MODIFIER_MISMATCH.Format(VariableModifier.COPY, target.Modifier));
        
        UserObjectInstance instance = target.GetValue<UserObjectInstance>();
        Variable? field = instance.Fields.FindVariable(m_field);
        if (field is null)
            throw new KeyNotFoundException(ExceptionConstants.VALUE_SEARCH_FAILED.Format(m_field));
        
        context.VariableStack.Push(field);
    }
}