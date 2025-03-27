using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class UnitExecutionContext(ExecutableUnit unit, TypeInfo expectedReturnType)
{
    public ExecutableUnit Unit { get; } = unit;
    public TypeInfo ExpectedReturnType { get; } = expectedReturnType;
    public int InstructionPointer { get; set; }
    public Instruction NextInstruction => Unit.Instructions[InstructionPointer];
    public bool IsComplete => InstructionPointer >= Unit.Instructions.Count;
    public int StackRestoreTarget { get; set; }
    public Variable? ValueToPushOnPop { get; set; }

    /// <summary>
    /// Gets the next instruction and advances the <see cref="InstructionPointer"/>
    /// </summary>
    /// <returns></returns>
    public Instruction StepNextInstruction()
    {
        return Unit.Instructions[InstructionPointer++];
    }

    public SourceLocation UnitLocation => Unit.SourceLocation;
}