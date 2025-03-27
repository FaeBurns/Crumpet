using System.Numerics;
using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Unary;

public class NegativeNumberInstruction : Instruction
{
    public NegativeNumberInstruction(SourceLocation location) : base(location)
    {
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable number = context.VariableStack.Pop();

        Variable result;
        if (number.Type is BuiltinTypeInfo<int>)
            result = MakeNegativeNumber<int>(number);
        else if (number.Type is BuiltinTypeInfo<float>)
            result = MakeNegativeNumber<float>(number);
        else
            throw new TypeMismatchException(ExceptionConstants.INVALID_TYPE.Format("int|float", number.Type));
        
        context.VariableStack.Push(result);
    }

    private Variable MakeNegativeNumber<T>(Variable number) where T : INumber<T>
    {
        T num = number.GetValue<T>();
        return Variable.Create(number.Type, -num);
    }
}