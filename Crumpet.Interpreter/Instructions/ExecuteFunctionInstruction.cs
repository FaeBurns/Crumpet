using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Instructions;

public class ExecuteFunctionInstruction : IInstruction
{
    private readonly FunctionDefinition m_targetFunction;
    private readonly bool m_pushReturnValue;
    
    /// <param name="targetFunction">The function to call.</param>
    /// <param name="pushReturnValue">Should the return value be pushed onto the stack?. Ignored if <paramref name="targetFunction"/>returns <see cref="VoidTypeInfo"/>.</param>
    public ExecuteFunctionInstruction(FunctionDefinition targetFunction, bool pushReturnValue)
    {
        m_targetFunction = targetFunction;
        m_pushReturnValue = pushReturnValue;

        if (targetFunction.ReturnType is VoidTypeInfo)
            m_pushReturnValue = false;
    }
    
    public void Execute()
    {
        throw new NotImplementedException();
    }
}