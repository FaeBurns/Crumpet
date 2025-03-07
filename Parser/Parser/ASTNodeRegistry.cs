using System.Reflection;
using Lexer;
using Parser.Nodes;
using Shared;
using Shared.Collections;
using Shared.Exceptions;

namespace Parser;

public class ASTNodeRegistry<TToken> where TToken : Enum
{
    private readonly MultiDictionary<Type, NonTerminalDefinition> m_nonTerminalDefinitions = new MultiDictionary<Type, NonTerminalDefinition>();
    private readonly Dictionary<TToken, TerminalDefinition<TToken>> m_terminalDefinitions = new Dictionary<TToken, TerminalDefinition<TToken>>();

    private static readonly ConstructorInfo s_defaultTerminalConstructor = typeof(TerminalNode<TToken>).GetConstructor([typeof(Token<TToken>)])!;

    public void RegisterTerminal<T>() where T : ITerminalNodeFactory<TToken>
    {
        foreach (TerminalDefinition<TToken> terminal in T.GetTerminals())
        {
            m_terminalDefinitions.Add(terminal.Token, terminal);
        }
    }

    public void RegisterNonTerminal<T>() where T : INonTerminalNodeFactory
    {
        foreach (NonTerminalDefinition nonTerminal in T.GetNonTerminals())
        {
            m_nonTerminalDefinitions.Add(nonTerminal.Type, nonTerminal);
        }
    }

    public void RegisterFactoryCollection<T>() where T : INodeFactoryCollection, new()
    {
        INodeFactoryCollection collection = new T();

        foreach (Type terminalType in collection.GetTerminalFactories())
        {
            if (!typeof(ITerminalNodeFactory<TToken>).IsAssignableFrom(terminalType))
                throw new ParserDefinitionException(ExceptionConstants.PARSER_INVALID_FACTORY_ELEMENT.Format(terminalType, typeof(ITerminalNodeFactory<TToken>)));

            // get collection method on factory using reflection
            MethodInfo registryMethod = terminalType.GetMethod(nameof(ITerminalNodeFactory<TToken>.GetTerminals))!;

            // invoke and iterate to register
            IEnumerable<TerminalDefinition<TToken>> definitions = (IEnumerable<TerminalDefinition<TToken>>)registryMethod.Invoke(null, Array.Empty<object>())!;
            foreach (TerminalDefinition<TToken> definition in definitions)
            {
                m_terminalDefinitions.Add(definition.Token, definition);
            }
        }

        foreach (Type nonTerminalType in collection.GetNonTerminalFactories())
        {
            if (!typeof(INonTerminalNodeFactory).IsAssignableFrom(nonTerminalType))
                throw new ParserDefinitionException(ExceptionConstants.PARSER_INVALID_FACTORY_ELEMENT.Format(nonTerminalType, typeof(INonTerminalNodeFactory)));

            // get collection method on factory using reflection
            MethodInfo registryMethod = nonTerminalType.GetMethod(nameof(INonTerminalNodeFactory.GetNonTerminals))!;

            // invoke and iterate to register
            IEnumerable<NonTerminalDefinition> definitions = (IEnumerable<NonTerminalDefinition>)registryMethod.Invoke(null, Array.Empty<object>())!;
            foreach (NonTerminalDefinition definition in definitions)
            {
                m_nonTerminalDefinitions.Add(definition.Type, definition);
            }
        }
    }

    public TerminalDefinition<TToken>? FindTerminalDefinition(TToken token)
    {
        return m_terminalDefinitions.GetValueOrDefault(token);
    }

    public IEnumerable<NonTerminalDefinition>? FindNonTerminalDefinitions(Type type)
    {
        if (m_nonTerminalDefinitions.TryGetValue(type, out List<NonTerminalDefinition>? definitions))
        {
            return definitions;
        }

        return null;
    }

    public TerminalDefinition<TToken> GetTerminalDefinition(TToken token)
    {
        TerminalDefinition<TToken>? definition = FindTerminalDefinition(token);
        if (definition is null)
            throw new ArgumentException(ExceptionConstants.PARSER_UNKOWN_TERMINAL.Format(token));

        return definition;
    }

    public IEnumerable<NonTerminalDefinition> GetNonTerminalDefinitions(Type type)
    {
        IEnumerable<NonTerminalDefinition>? definitions = FindNonTerminalDefinitions(type);
        if (definitions is null)
            throw new ArgumentException(ExceptionConstants.PARSER_UNKOWN_NONTERMINAL.Format(type));

        return definitions;
    }

    public ConstructorInfo GetNodeConstructorForToken(TToken token)
    {
        TerminalDefinition<TToken>? definition = FindTerminalDefinition(token);
        if (definition is not null)
            return definition.Constructor;

        return s_defaultTerminalConstructor;
    }

    public IEnumerable<TerminalDefinition<TToken>> GetTerminals() => m_terminalDefinitions.Select(i => i.Value);
    public IEnumerable<NonTerminalDefinition> GetNonTerminals() => m_nonTerminalDefinitions.SelectMany(i => i.Value);
}