using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions;

public class PushTypeIdentifierConstantInstruction : Instruction
{
    private readonly string m_typeName;
    private readonly int m_genericArgCount;

    public PushTypeIdentifierConstantInstruction(string typeName, int genericArgCount, SourceLocation location) : base(location)
    {
        m_typeName = typeName;
        m_genericArgCount = genericArgCount;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        // pop off the stack into an array
        // do it in reverse to get them in the correct order for the ResolveType function
        // ResolveType expects them in the human-readable order, wheras the stack will have them in the reverse
        TypeInfo[] genericArgs = new TypeInfo[m_genericArgCount];
        for (int i = m_genericArgCount - 1; i >= 0; i--)
        {
            genericArgs[i] = context.VariableStack.Pop().GetValue<TypeInfo>();
        }
        
        TypeInfo? type = context.TypeResolver.ResolveType(m_typeName, genericArgs);
        if (type is null)
            throw new TypeMismatchException(ExceptionConstants.UNKOWN_TYPE.Format(m_typeName));
        
        context.VariableStack.Push(new TypeTypeInfo(type), type);
    }
}