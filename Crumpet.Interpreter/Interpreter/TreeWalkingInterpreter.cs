using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Interpreter;

public class TreeWalkingInterpreter<T> where T : Enum
{
    public Scope CurrentScope { get; private set; } = new Scope(null);
    
    public TreeWalkingInterpreter(NonTerminalNode root)
    {
        
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