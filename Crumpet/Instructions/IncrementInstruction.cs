﻿using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Instructions;

public class IncrementInstruction : Instruction
{
    private readonly bool m_add;

    public IncrementInstruction(bool add, SourceLocation location) : base(location)
    {
        m_add = add;
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable variable = context.VariableStack.Pop();

        if (variable.Type is BuiltinTypeInfo<int>)
        {
            variable.SetValue(variable.GetValue<int>() + (m_add ? 1 : -1));
        }

        if (variable.Type is BuiltinTypeInfo<float>)
        {
            variable.SetValue(variable.GetValue<float>() + (m_add ? 1 : -1f));
        }
        
        // push it again?
        // will lead to a leak if it occurs too much
        // if it's in a for finalizer it'll save the stack count then unwind to restore it at the end of the finalizer
        // but just in case the value is wanted still
        // this needs to occur
        context.VariableStack.Push(variable);
    }
}