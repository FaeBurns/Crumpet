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

    public void OnPush(InterpreterExecutionContext context)
    {
        if (unit.TypeArgs.Any())
        {
            context.TypeResolver.PushGenericArguments(new GenericTypeContext(unit.TypeArgs));
        }
        else if (unit.BlocksScope)
        {
            // push an empty context if it only blocks the scope
            // don't want type arguments being read from somewhere above
            context.TypeResolver.PushGenericArguments(new GenericTypeContext());
        }
    }

    public void OnPop(InterpreterExecutionContext context)
    {
        if (unit.TypeArgs.Any())
        {
            context.TypeResolver.PopGenericArguments();
        }
    }
}