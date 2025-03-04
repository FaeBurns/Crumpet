using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Interpreter.Preparse;
using Crumpet.Interpreter.SequenceOperations;

namespace Crumpet.Interpreter;

public class TreeWalkingInterpreter
{
    public Scope CurrentScope { get; private set; } = new Scope(null);

    private IInstruction[] m_programSequence;
    public TypeResolver TypeResolver { get; }

    public TreeWalkingInterpreter(NonTerminalNode root)
    {
        IEnumerable<ASTNode> nodeSequence = new NodeSequenceEnumerator(root);

        // ReSharper disable once SuspiciousTypeConversion.Global
        m_programSequence = nodeSequence.OfType<IInstructionProvider>().SelectMany(n => n.GetInstructions()).ToArray();

        TypeResolver = new TypeBuilder(root).GetTypeDefinitions();

        // list to be populated
        List<IInstruction> sequenceOperations = new List<IInstruction>();

        // populate list from root node
        // root.BuildProgram(sequenceOperations);
        
        // create 
        m_programSequence = sequenceOperations.ToArray();
    }

    public void Step()
    {
        
    }

    public void PushScope()
    {
        CurrentScope = new Scope(CurrentScope);
    }

    public void PopScope()
    {
        CurrentScope = CurrentScope?.Parent ?? throw new InvalidOperationException(ExceptionConstants.POP_SCOPE_FAILED);
    }
}