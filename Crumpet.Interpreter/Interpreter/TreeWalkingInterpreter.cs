using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Preparse;
using Crumpet.Parser;
using Crumpet.Parser.Nodes;

namespace Crumpet.Interpreter;

public class TreeWalkingInterpreter
{
    public Scope CurrentScope { get; private set; } = new Scope(null);

    public FunctionCollection Functions { get; }
    public TypeResolver TypeResolver { get; }

    public TreeWalkingInterpreter(NonTerminalNode root)
    {
        IEnumerable<ASTNode> nodeSequence = new NodeSequenceEnumerator(root);
        TypeResolver = new TypeBuilder(nodeSequence).GetTypeDefinitions();
        Functions = new FunctionCollection(nodeSequence);
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