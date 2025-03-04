namespace Crumpet;

/// <summary>
/// A De-Duplicated directed graph of value types. Must always contain at least one value
/// </summary>
/// <typeparam name="T">The type the graph contains</typeparam>
public class ValueGraph<T> where T : struct
{
    private readonly Dictionary<T, Node> m_nodes = new Dictionary<T, Node>();
    
    public ValueGraph(T root)
    {
        m_nodes.Add(root, new Node(root));
    }
    
    public void Add(T parent, T child)
    {
        // do nothing if child already added
        if (EnumerateChildren(parent).Contains(child))
            return;
        
        // create child node, add it to reference, and then add in-node connections
        Node childNode = new Node(child);
        m_nodes.Add(child, childNode);
        m_nodes[parent].Children.Add(childNode);
        childNode.Parents.Add(m_nodes[parent]);
    }

    public void Remove(T value)
    {
        // return if not present
        if (!m_nodes.TryGetValue(value, out Node? node))
            return;
        
        // remove from parents and children
        node.Parents.ForEach(n => n.Children.Remove(node));
        node.Children.ForEach(n => n.Parents.Remove(node));

        // remove from nodes map to clear from graph
        m_nodes.Remove(value);
    }

    public IEnumerable<T> EnumerateChildren(T parent)
    {
        return GetNode(parent)?.Children.Select(n => n.Value) ?? throw new KeyNotFoundException(ExceptionConstants.KEY_NOT_FOUND.Format(parent));
    }

    private Node? GetNode(T value)
    {
        return m_nodes.GetValueOrDefault(value);
    }
    
    private class Node
    {
        public Node(T value)
        {
            Value = value;
        }

        public T Value { get; }
        
        public List<Node> Children { get; } = new List<Node>();
        public List<Node> Parents { get; } = new List<Node>();
    }
}