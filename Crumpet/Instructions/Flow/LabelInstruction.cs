using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Shared;

namespace Crumpet.Instructions.Flow;

public class LabelInstruction : Instruction
{
    public Guid ID { get; }
    public required string FriendlyName { get; init; }

    public LabelInstruction(Guid id, SourceLocation location) : base(location)
    {
        ID = id;
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