using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Interpreter;

public class VariableStack
{
    private readonly Stack<Variable> m_variables = new Stack<Variable>();

    public void Push(Variable value)
    {
        m_variables.Push(value);
    }

    public Variable Pop()
    {
        if (m_variables.Count == 0)
            throw new InvalidOperationException(ExceptionConstants.VARIABLE_STACK_POP_WHILE_EMPTY);

        return m_variables.Pop();
    }
}