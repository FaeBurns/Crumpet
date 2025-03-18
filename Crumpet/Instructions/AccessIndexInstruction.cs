using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions;

public class AccessIndexInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable index = context.VariableStack.Pop();
        Variable target = context.VariableStack.Pop();

        if (index.Type is not ArrayTypeInfo)
            throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format(typeof(ArrayTypeInfo), index.Type));
        
        if (!target.AssertType<int>())
            throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format(typeof(int), target.Type));

        // get array object and get variable at desired index
        IList<Variable> array = target.GetValue<IList<Variable>>();
        int indexInto = index.GetValue<int>();
        Variable elementVariable = array[indexInto];
        
        // push that variable to the stack
        context.VariableStack.Push(elementVariable);
    }
}