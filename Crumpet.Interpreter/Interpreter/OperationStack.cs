using Crumpet.Interpreter.Interpreter.SequenceOperations;
using Crumpet.Interpreter.Interpreter.Variables;

namespace Crumpet.Interpreter.Interpreter;

public class OperationStack
{
    public void PushVariable(Variable value)
    {
        
    }

    public Variable PopVariable()
    {
        throw new NotImplementedException();
    }

    public void PushOperation(IProgramNode operation)
    {
        
    }

    public IProgramNode PopOperation()
    {
        throw new NotImplementedException();
    }
}