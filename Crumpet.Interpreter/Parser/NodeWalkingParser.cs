namespace Crumpet.Interpreter.Parser;

public class NodeWalkingParser<T> : IParser where T : Enum
{
    private readonly ASTNodeRegistry<T> m_nodeRegistry;

    public NodeWalkingParser(ASTNodeRegistry<T> nodeRegistry)
    {
        m_nodeRegistry = nodeRegistry;
    }
}