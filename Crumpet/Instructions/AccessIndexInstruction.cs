using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions;

public class AccessIndexInstruction : Instruction
{
    public AccessIndexInstruction(SourceLocation location) : base(location)
    {
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable index = context.VariableStack.Pop();
        Variable target = context.VariableStack.Pop();

        if (target.Type is not ArrayTypeInfo and not BuiltinTypeInfo<string>)
            throw new TypeMismatchException(ExceptionConstants.INVALID_TYPE.Format($"{typeof(ArrayTypeInfo)}|{typeof(string)}", target.Type));
        
        if (!index.AssertType<int>())
            throw new TypeMismatchException(ExceptionConstants.INVALID_TYPE.Format(typeof(int), index.Type));

        int indexInto = index.GetValue<int>();
        if (target.Type is ArrayTypeInfo)
        {
            // get array object and get variable at desired index
            IList<Variable> array = target.GetValue<IList<Variable>>();
            Variable elementVariable = array[indexInto];
        
            // push that variable to the stack
            context.VariableStack.Push(elementVariable);
        }
        else if (target.Type is BuiltinTypeInfo<string>)
        {
            string str = target.GetValue<string>();
            string result = str[indexInto].ToString();
            context.VariableStack.Push(Variable.Create(BuiltinTypeInfo.String, result));
        }
        else
        {
            throw new UnreachableException();
        }
    }
}