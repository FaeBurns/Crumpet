using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter;

public class VariableStack
{
    private readonly Stack<Variable> m_variables = new Stack<Variable>();

    public int Count => m_variables.Count();

    public void Push(TypeInfo type, object? value) => Push(Variable.Create(type, value));

    public void Push(Variable value)
    {
        m_variables.Push(value);
    }

    public Variable Pop()
    {
        if (m_variables.Count == 0)
            throw new ArgumentException(ExceptionConstants.VARIABLE_STACK_POP_WHILE_EMPTY);

        return m_variables.Pop();
    }

    public Variable Peek()
    {
        if (m_variables.Count == 0)
            throw new ArgumentException(ExceptionConstants.VARIABLE_STACK_PEEK_WHILE_EMPTY);

        return m_variables.Peek();
    }

    /// <summary>
    /// Peeks the first <paramref name="count"/> elements on the stack. Returns them in the order they would be popped in.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IEnumerable<Variable> Peek(int count)
    {
        if (m_variables.Count < count)
            throw new ArgumentException(ExceptionConstants.VARIABLE_STACK_PEEK_INSUFFICIENT_COUNT.Format(count, m_variables.Count));
        
        List<Variable> result = new List<Variable>(count);
        for (int i = 0; i < count; i++)
        {
            result.Add(m_variables.Pop());
        }

        // re-add to the stack
        // have to cast to IEnumerable first as List overrides Reverse to be in place where we just want to iterate in reverse
        foreach (Variable variable in ((IEnumerable<Variable>)result).Reverse())
        {
            m_variables.Push(variable);
        }

        return result;
    }

    public void UnwindTo(int targetCount)
    {
        while (m_variables.Count > targetCount)
        {
            m_variables.Pop();
        }
    }
}