namespace Crumpet.Interpreter.Interpreter.SequenceOperations;

public interface IProgramNode
{
    public void Evaluate(List<ISequenceOperation> programSequence);
}