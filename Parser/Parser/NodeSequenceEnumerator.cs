using System.Collections;
using Parser.Nodes;

namespace Parser;

public class NodeSequenceEnumerator : IEnumerable<ASTNode>
{
    private readonly NonTerminalNode m_root;

    public NodeSequenceEnumerator(NonTerminalNode root)
    {
        m_root = root;
    }

    public IEnumerator<ASTNode> GetEnumerator()
    {
        Stack<ASTNode> stack = new Stack<ASTNode>();
        stack.Push(m_root);
        while (stack.Any())
        {
            ASTNode node = stack.Pop();
            
            // yield node immediately
            yield return node;

            // push children to stack
            if (node is NonTerminalNode nonTerminalNode)
            {
                foreach (ASTNode child in nonTerminalNode.EnumerateChildren()) 
                    stack.Push(child);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}