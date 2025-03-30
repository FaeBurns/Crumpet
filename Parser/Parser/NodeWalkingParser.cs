using Lexer;
using Parser.Nodes;
using Shared;
using Shared.Exceptions;

namespace Parser;

public class NodeWalkingParser<T, TRoot> where T : Enum where TRoot : ASTNode
{
    private readonly ASTNodeRegistry<T> m_nodeRegistry;
    private readonly NodeTypeTree<T> m_nodeTree;

    public NodeWalkingParser(ASTNodeRegistry<T> nodeRegistry, NodeTypeTree<T> nodeTree)
    {
        m_nodeRegistry = nodeRegistry;
        m_nodeTree = nodeTree;
    }

    public ParseResult<T, TRoot> ParseToRoot(IEnumerable<Token<T>> tokens)
    {
        List<TerminalNode<T>> terminals = new List<TerminalNode<T>>();

        foreach (Token<T> token in tokens)
        {
            // null case occurs if a token has been used but is not defined in the nodes and in the tree somewhere
            NodeTypeTree<T>.TerminalNodeDefinition? terminal = m_nodeTree.GetNodeForTerminal(token.TokenId);
            if (terminal is null)
                throw new ParserException(ExceptionConstants.MISSING_TERMINAL_NODE.Format(token.TokenId), token.Location);

            // invoke constructor for relevant terminal node
            TerminalNode<T> node = (TerminalNode<T>)m_nodeRegistry.GetNodeConstructorForToken(token.TokenId).Invoke([token]);

            // add to ordered node list
            terminals.Add(node);
        }

        ObjectStream<TerminalNode<T>> terminalStream = new ObjectStream<TerminalNode<T>>(terminals);
        IEnumerable<NonTerminalDefinition> rootNodeDefinitions = m_nodeRegistry.GetNonTerminalDefinitions(typeof(TRoot));

        foreach (NonTerminalDefinition definition in rootNodeDefinitions)
        {
            NonTerminalInstanceConstructor<T> constructor = new NonTerminalInstanceConstructor<T>(definition);
            if (constructor.Construct(terminalStream, m_nodeRegistry) is TRoot node)
            {
                // if stream is at the end
                if (terminalStream.Position == terminalStream.Length)
                    // return the last element
                    return new ParseResult<T, TRoot>(node, terminalStream[^1], true);
                else
                    // otherwise return the next item
                    return new ParseResult<T, TRoot>(node, terminalStream[terminalStream.HighestPosition], false);
            }
        }

        return new ParseResult<T, TRoot>(null, terminalStream[terminalStream.HighestPosition], false);
    }
}

public class ParseResult<T, TRoot> where T : Enum where TRoot : ASTNode
{
    public ParseResult(TRoot? root, TerminalNode<T> lastTerminalHit, bool success)
    {
        Root = root;
        LastTerminalHit = lastTerminalHit;
        Success = success;
    }

    public TRoot? Root { get; }
    
    public TerminalNode<T> LastTerminalHit { get; }

    public bool Success { get; }
}