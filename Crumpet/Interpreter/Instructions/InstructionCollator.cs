using System.Collections;
using System.Diagnostics;
using Crumpet.Instructions;

namespace Crumpet.Interpreter.Instructions;

public class InstructionCollator : IEnumerable<Instruction>
{
    private readonly IEnumerable m_instructionsEnumerable;

    public InstructionCollator(IInstructionProvider? instructionProvider) : this(instructionProvider?.GetInstructionsRecursive() ?? Array.Empty<object>())
    {
    }

    public InstructionCollator(IEnumerable instructionsEnumerable)
    {
        m_instructionsEnumerable = instructionsEnumerable;
    }
    
    public IEnumerator<Instruction> GetEnumerator()
    {
        List<Instruction> result = new List<Instruction>();
        EnumerateStep(m_instructionsEnumerable, result);
        return result.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void EnumerateStep(IEnumerable enumerator, List<Instruction> target)
    {
        foreach (object step in enumerator)
        {
            switch (step)
            {
                case null: // ignore if null
                    continue;
                case IEnumerable innerEnumerable: // yielding a collection or collator
                    EnumerateStep(innerEnumerable, target);
                    break;
                case IInstructionProvider instructionProvider: // yielding a node
#if DEBUG
                    target.Add(new DebugReferenceProviderInstruction(instructionProvider));
#endif
                    EnumerateStep(instructionProvider.GetInstructionsRecursive(), target);
                    break;
                case Instruction instruction: // yielding an instruction
                    target.Add(instruction);
                    break;
                default:
                    throw new UnreachableException();
            }
        }
    }
}