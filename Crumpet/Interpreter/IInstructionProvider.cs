using Crumpet.Interpreter.Instructions;

namespace Crumpet.Interpreter;

public interface IInstructionProvider
{
    public IEnumerable<Instruction> GetInstructions();
}