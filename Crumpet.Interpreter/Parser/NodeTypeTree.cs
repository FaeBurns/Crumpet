using System.Text;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Lexer;
using Crumpet.Interpreter.Parser.NodeConstraints;

namespace Crumpet.Interpreter.Parser;

public class NodeTypeTree<T> where T : Enum
{
    private readonly Dictionary<Type, NonTerminalNodeDefinition> m_nonTerminalNodeCache = new Dictionary<Type, NonTerminalNodeDefinition>();
    
    private readonly ASTNodeRegistry<T> m_registry;
    private readonly NonTerminalNodeDefinition m_rootNodeDefinition;
    
    public NodeTypeTree(ASTNodeRegistry<T> registry, Type rootNodeType)
    {
        m_registry = registry;
        m_rootNodeDefinition = BuildTree(rootNodeType);
    }
    
    public NonTerminalNodeDefinition GetRootNode()
    {
        return m_rootNodeDefinition;
    }

    private NonTerminalNodeDefinition BuildTree(Type rootNodeType)
    {
        IReadOnlyList<NonTerminalDefinition> nonTerminalDefinitions = m_registry.GetNonTerminals().ToList();
        IReadOnlyList<TerminalDefinition<T>> terminalDefinitions = m_registry.GetTerminals().ToList();
        
        // throw if no nodes are registered
        if (!(nonTerminalDefinitions.Any() || terminalDefinitions.Any()))
            throw new ArgumentException(ExceptionConstants.NO_NODES_REGISTERED);

        NonTerminalNodeDefinition rootNodeDefinition = DefineNonTerminalByType(rootNodeType);
        
        return rootNodeDefinition;
    }

    private NonTerminalNodeDefinition DefineNonTerminalByType(Type type)
    {
        // exit early if cached instance already exists
        if (m_nonTerminalNodeCache.TryGetValue(type, out NonTerminalNodeDefinition? cachedNodeDefinition))
            return cachedNodeDefinition;
        
        // try and find root node by name
        IReadOnlyList<NonTerminalDefinition> nonTerminalDefinitions = m_registry.GetNonTerminalDefinitions(type).ToList();
        if (!nonTerminalDefinitions.Any())
            throw new ArgumentException(ExceptionConstants.INVALID_NODE_NAME.Format(type));
        
        // delcaring type is never null
        NonTerminalNodeDefinition nodeDefinition = new NonTerminalNodeDefinition(type, nonTerminalDefinitions);
        m_nonTerminalNodeCache[type] = nodeDefinition; // add to cache immediately to prevent issues with recursion
        
        // walk constraint tree and get all non-terminals and terminals used
        HashSet<Type> nonTerminalTypes = new HashSet<Type>();
        HashSet<T> terminals = new HashSet<T>();
        foreach (NonTerminalDefinition nonTerminalDefinition in nonTerminalDefinitions)
        {
            nonTerminalTypes.AddRange(GetNonTerminalsInConstraint(nonTerminalDefinition.Constraint));
            terminals.AddRange(GetTerminalsInConstraint(nonTerminalDefinition.Constraint));
        }

        // loop through all non-terminals and define them
        // warning - recursive
        // is not an issue with recursive constraints as the node reference has already been cached
        foreach (Type nonTerminalType in nonTerminalTypes)
        {
            NonTerminalNodeDefinition child = DefineNonTerminalByType(nonTerminalType);
            nodeDefinition.NonTerminalChildren.AddLast(child);
            
            // no idea if adding to parent can happen multiple times but check anyway
            if (!child.Parents.Contains(nodeDefinition))
                child.Parents.AddLast(nodeDefinition);
        }

        // populate with terminals
        foreach (T terminal in terminals) 
            nodeDefinition.TerminalChildren.AddLast(terminal);
        
        return nodeDefinition;
    }

    /// <summary>
    /// Walks the constraint tree to get all referenced non-terminals.
    /// </summary>
    /// <param name="constraint">The root node of the constraint tree.</param>
    /// <returns>An iterator over all found non-terminal types.</returns>
    private IEnumerable<Type> GetNonTerminalsInConstraint(NodeConstraint constraint)
    {
        switch (constraint)
        {
            case MultiNodeConstraint multiNodeConstraint:
                foreach (NodeConstraint nodeConstraint in multiNodeConstraint.Constraints)
                    foreach (Type nonTerminal in GetNonTerminalsInConstraint(nodeConstraint))
                        yield return nonTerminal;
                break;
            case ContainsSingleConstraint singleConstraint:
                foreach (Type nonTerminal in GetNonTerminalsInConstraint(singleConstraint.Constraint))
                    yield return nonTerminal;
                break;
            case NonTerminalConstraint nonTerminalConstraint:
                yield return nonTerminalConstraint.NonTerminalType;
                break;
        }
    }

    /// <summary>
    /// Walks the constraint tree to get all referenced terminals.
    /// </summary>
    /// <param name="constraint">The root node of the constraint tree.</param>
    /// <returns>An iterator over all found terminals.</returns>
    private IEnumerable<T> GetTerminalsInConstraint(NodeConstraint constraint)
    {
        switch (constraint)
        {
            case MultiNodeConstraint multiNodeConstraint:
                foreach (NodeConstraint nodeConstraint in multiNodeConstraint.Constraints)
                foreach (T nonTerminal in GetTerminalsInConstraint(nodeConstraint))
                    yield return nonTerminal;
                break;
            case ContainsSingleConstraint singleConstraint:
                foreach (T nonTerminal in GetTerminalsInConstraint(singleConstraint.Constraint))
                    yield return nonTerminal;
                break;
            case RawTerminalConstraint<T> rawTerminalConstraint:
                yield return rawTerminalConstraint.Token;
                break;
            case TerminalConstraint<T> namedTerminalConstraint:
                yield return namedTerminalConstraint.Token;
                break;
        }
    }
    
    public class NonTerminalNodeDefinition(Type type, IEnumerable<NonTerminalDefinition> rules)
    {
        public readonly Type Type = type;
        public readonly List<NonTerminalDefinition> Rules = rules.ToList();

        public readonly LinkedList<NonTerminalNodeDefinition> NonTerminalChildren = new LinkedList<NonTerminalNodeDefinition>();
        public readonly LinkedList<T> TerminalChildren = new LinkedList<T>();
        public readonly LinkedList<NonTerminalNodeDefinition> Parents = new LinkedList<NonTerminalNodeDefinition>();

        // print out the node as ebnf
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Type.Name.Replace("Node", String.Empty));
            builder.Append("\t\t\t: ");
            foreach (NonTerminalDefinition rule in rules)
            {
                builder.AppendLine(rule.Constraint.ToString());
                builder.Append("\t\t\t| ");
            }

            // remove last 2 characters '|' and ' ' 
            builder.Length -= 2;
            builder.Append(';');

            return builder.ToString();
        }
    }

    /// <summary>
    /// Prints out the entire tree
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        HashSet<NonTerminalNodeDefinition> nonTerminals = new HashSet<NonTerminalNodeDefinition>();
        HashSet<T> terminals = new HashSet<T>();
        
        void RecursiveAddNodeToSet(NonTerminalNodeDefinition node)
        {
            if (nonTerminals.Contains(node))
                return;
            nonTerminals.Add(node);
            
            foreach (NonTerminalNodeDefinition nonTerminal in node.NonTerminalChildren)
            {
                RecursiveAddNodeToSet(nonTerminal);
            }

            foreach (T token in node.TerminalChildren)
            {
                terminals.Add(token);
            }
        }

        void AppendTerminal(T token)
        {
            builder.AppendLine(token.ToString());
            builder.Append("\t\t\t: '");
            TokenAttribute tokenAttribute = token.GetEnumAttribute<TokenAttribute>();
            builder.Append(tokenAttribute.Regex);
            builder.AppendLine("'");
            builder.AppendLine("\t\t\t;\n");
        }
        
        RecursiveAddNodeToSet(m_rootNodeDefinition);

        nonTerminals.Foreach(n => builder.AppendLine(n.ToString() + "\n"));
        terminals.Foreach(AppendTerminal);
        
        return builder.ToString();
    }
}