namespace Crumpet.Parser.Nodes;

public abstract class NonTerminalNode : ASTNode
{
    private readonly IEnumerable<ASTNode?> m_implicitChildren;

    protected NonTerminalNode()
    {
        m_implicitChildren = Array.Empty<ASTNode>();
    }

    protected NonTerminalNode(params IEnumerable<ASTNode?> implicitChildren)
    {
        m_implicitChildren = implicitChildren;
    }

    public IEnumerable<ASTNode> EnumerateChildren()
    {
        foreach (ASTNode? node in m_implicitChildren)
            if (node is not null)
                yield return node;

        foreach (ASTNode? node in EnumerateChildrenDerived())
            if (node is not null)
                yield return node;
    }

    protected virtual IEnumerable<ASTNode?> EnumerateChildrenDerived() { yield break; }

    public override IEnumerable<object> TransformForConstructor()
    {
        // all nodes should just return themselves
        return [this];
    }
}