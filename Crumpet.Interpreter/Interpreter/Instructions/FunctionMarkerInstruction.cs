namespace Crumpet.Interpreter.Instructions;

/// <summary>
/// Marks the position of a function in an instruction sequence
/// </summary>
public class FunctionMarkerInstruction : IInstruction
{
    public FunctionMarkerInstruction(FunctionDefinition definition)
    {
        
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }
}