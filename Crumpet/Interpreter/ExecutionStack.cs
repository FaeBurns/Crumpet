﻿using Crumpet.Interpreter.Functions;

namespace Crumpet.Interpreter;

public class ExecutionStack
{
    private readonly Stack<UnitExecutionContext> m_units = new Stack<UnitExecutionContext>();
    private readonly InterpreterExecutionContext m_context;

    public ExecutionStack(InterpreterExecutionContext context)
    {
        m_context = context;
    }

    public void Push(UnitExecutionContext unit)
    {
        unit.StackRestoreTarget = m_context.VariableStack.Count;
        m_units.Push(unit);
        unit.OnPush(m_context);
    }

    public UnitExecutionContext Pop()
    {
        UnitExecutionContext unit = m_units.Peek();
        m_context.VariableStack.UnwindTo(unit.StackRestoreTarget);
        if (unit.ValueToPushOnPop is not null)
            m_context.VariableStack.Push(unit.ValueToPushOnPop);

        unit.OnPop(m_context);
        m_units.Pop();
        return unit; // is the same unit as the pop
    }
    
    public UnitExecutionContext Peek() => m_units.Peek();

    public bool Any() => m_units.Any();
    public void Clear() => m_units.Clear();
}