using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions;

public class PushTypeIdentifierConstantInstruction : Instruction
{
    private readonly string m_typeName;

    public PushTypeIdentifierConstantInstruction(string typeName, SourceLocation location) : base(location)
    {
        m_typeName = typeName;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        TypeInfo? type = context.TypeResolver.ResolveType(m_typeName);
        if (type is null)
            throw new InterpreterException(context, ExceptionConstants.UNKOWN_TYPE.Format(m_typeName));
        
        context.VariableStack.Push(new TypeTypeInfo(type), type);
    }
}