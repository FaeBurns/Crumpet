using System.Collections;

namespace Crumpet.Interpreter.Instructions;

public interface IInstructionProvider
{
    public IEnumerable GetInstructionsRecursive();
}