using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;

namespace Crumpet.Instructions.Flow;

public class LabelInstruction : Instruction
{
    public Guid ID { get; }
    public string FriendlyName { get; }

    public LabelInstruction(Guid id, string friendlyName = "")
    {
        ID = id;
        FriendlyName = friendlyName;
    }

    public override string ToString()
    {
        if (String.IsNullOrEmpty(FriendlyName))
            return nameof(LabelInstruction);
        else
            return $"{nameof(LabelInstruction)}: {FriendlyName}";
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        // do nothing on execute
    }
}