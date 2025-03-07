using Crumpet.Interpreter.Instructions;

namespace Crumpet.Interpreter.SequenceOperations;

public interface IInstructionProvider
{
    public IEnumerable<Instruction> GetInstructions();
}