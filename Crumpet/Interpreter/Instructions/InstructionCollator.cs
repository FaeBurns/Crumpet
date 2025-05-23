﻿using System.Diagnostics;
using Parser;

namespace Crumpet.Interpreter.Instructions;

public class InstructionCollator : IEnumerable<Instruction>
{
    private readonly IEnumerable m_instructionsEnumerable;

    public InstructionCollator(IInstructionProvider? instructionProvider) : this(instructionProvider?.GetInstructionsRecursive() ?? Enumerable.Empty<Instruction>())
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
                    EnumerateStep(instructionProvider.GetInstructionsRecursive(), target);
                    break;
                case Instruction instruction: // yielding an instruction
                    target.Add(instruction);
                    break;
                case ASTNode and not IInstructionProvider:
                    break;
                default:
                    throw new UnreachableException();
            }
        }
    }
}