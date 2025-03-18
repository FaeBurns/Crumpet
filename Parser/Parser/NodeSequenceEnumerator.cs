using System.Collections;
using Parser.Nodes;

namespace Parser;

public class NodeSequenceEnumerator
{
    public static IEnumerable<ASTNode> CreateSequential(NonTerminalNode root) => new SequentialNodeEnumerator(root);
    public static IEnumerable<ASTNode> CreateDepthFirst(NonTerminalNode root) => new DepthFirstNodeEnumerator(root);

    private class SequentialNodeEnumerator : IEnumerable<ASTNode>
    {
        private readonly NonTerminalNode m_root;

        public SequentialNodeEnumerator(NonTerminalNode root)
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

    public class DepthFirstNodeEnumerator : IEnumerable<ASTNode>
    {
        private readonly NonTerminalNode m_root;

        public DepthFirstNodeEnumerator(NonTerminalNode root)
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

                // push children to stack
                if (node is NonTerminalNode nonTerminalNode)
                {
                    foreach (ASTNode child in nonTerminalNode.EnumerateChildren())
                        stack.Push(child);
                }

                // yield node AFTER children
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}