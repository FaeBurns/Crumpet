using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Unary;

public class DereferencePointerInstruction : Instruction
{
    public DereferencePointerInstruction(SourceLocation location) : base(location)
    {
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable variable = context.VariableStack.Pop();
        Variable referencedVariable = variable.GetValue<Variable>();
        context.VariableStack.Push(referencedVariable);
    }
}