using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Preparse;
using Parser;
using Parser.Nodes;
using Shared;

namespace Crumpet.Interpreter;

public class TreeWalkingInterpreter
{
    public Scope CurrentScope { get; private set; } = new Scope(null);

    public FunctionCollection Functions { get; }
    public TypeResolver TypeResolver { get; }

    public TreeWalkingInterpreter(NonTerminalNode root)
    {
        ASTNode[] nodeSequence = NodeSequenceEnumerator.CreateSequential(root).ToArray();
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