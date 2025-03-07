using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;




namespace Crumpet.Instructions;

public class CreateVariableInstruction : Instruction
{
    private readonly VariableInfo m_variableInfo;

    public CreateVariableInstruction(VariableInfo variableInfo)
    {
        m_variableInfo = variableInfo;
    }

    public override void Execute(InterpreterExecutionContext context)
    {
        context.CurrentScope.Create(m_variableInfo);
    }
}