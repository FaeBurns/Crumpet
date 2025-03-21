﻿using System.Numerics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions.Unary;

public class NegativeNumberInstruction : Instruction
{
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable number = context.VariableStack.Pop();

        Variable result;
        if (number.Type is BuiltinTypeInfo<int>)
            result = MakeNegativeNumber<int>(number);
        if (number.Type is BuiltinTypeInfo<float>)
            result = MakeNegativeNumber<float>(number);
        else
            throw new InterpreterException(context, ExceptionConstants.INVALID_TYPE.Format("int|float", number.Type));
        
        context.VariableStack.Push(result);
    }

    private Variable MakeNegativeNumber<T>(Variable number) where T : INumber<T>
    {
        T num = number.GetValue<T>();
        return Variable.Create(number.Type, -num);
    }
}