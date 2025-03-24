using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions.Unary;

public class CreateReferenceToInstruction : Instruction
{
    public CreateReferenceToInstruction(SourceLocation location) : base(location)
    {
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        Variable variable = context.VariableStack.Pop();
        Variable reference = Variable.CreatePointer(variable.Type, variable);
        
        context.VariableStack.Push(reference);
    }
}