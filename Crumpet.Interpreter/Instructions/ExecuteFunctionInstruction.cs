﻿using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Instructions;

public class ExecuteFunctionInstruction : Instruction
{
    private readonly ExecutableUnit m_function;

    /// <param name="function">The function to call.</param>
    public ExecuteFunctionInstruction(ExecutableUnit function)
    {
        m_function = function;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        context.Call(m_function);
    }
}