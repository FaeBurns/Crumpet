namespace Crumpet.Interpreter.Parser.Nodes;

public abstract class TerminalNode<T> : ASTNode where T : Enum
{
    protected TerminalNode(string terminal)
    {
        Terminal = terminal;
    }
    
    public TerminalDefinition<T> TriggeredConstraint { get; internal set; } = null!;
    public string Terminal { get; internal set; }
}