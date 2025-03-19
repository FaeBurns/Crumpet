namespace Parser.Nodes;

public abstract class NonTerminalNode : ASTNode
{
    protected readonly List<ASTNode?> ImplicitChildren;

    protected NonTerminalNode()
    {
        ImplicitChildren = new List<ASTNode?>();
    }

    protected NonTerminalNode(params IEnumerable<ASTNode?> implicitChildren)
    {
        ImplicitChildren = implicitChildren.ToList();
    }

    public IEnumerable<ASTNode> EnumerateChildren()
    {
        foreach (ASTNode? node in ImplicitChildren)
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