namespace Crumpet.Interpreter.Parser.Nodes;

public abstract class TerminalNode : ASTNode
{
    protected TerminalNode(string terminal)
    {
        Terminal = terminal;
    }
    
    public TerminalDefinition TriggeredConstraint { get; internal set; } = null!;
    public string Terminal { get; internal set; }
}