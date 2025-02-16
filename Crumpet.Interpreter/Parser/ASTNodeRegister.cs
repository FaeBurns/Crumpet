using System.Reflection;
using Crumpet.Interpreter.Collections;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser;

public class ASTNodeRegister
{ 
    private readonly MultiDictionary<string, NonTerminalDefinition> m_nonTerminalDefinitions = new MultiDictionary<string, NonTerminalDefinition>();
    private readonly MultiDictionary<string, TerminalDefinition> m_terminalDefinitions = new MultiDictionary<string, TerminalDefinition>();

    public void RegisterTerminal<T>() where T : ITerminalNodeFactory
    {
        foreach (TerminalDefinition terminal in T.GetTerminals())
        {
            m_terminalDefinitions.Add(terminal.Name, terminal);
        }
    }

    public void RegisterNonTerminal<T>() where T : INonTerminalNodeFactory
    {
        foreach (NonTerminalDefinition nonTerminal in T.GetNonTerminals())
        {
            m_nonTerminalDefinitions.Add(nonTerminal.Name, nonTerminal);
        }
    }

    public void RegisterFactoryCollection<T>() where T : INodeFactoryCollection, new()
    {
        INodeFactoryCollection collection = new T();
        
        foreach (Type terminalType in collection.GetTerminalFactories())
        {
            if (!typeof(ITerminalNodeFactory).IsAssignableFrom(terminalType))
                throw new ParserDefinitionException(ExceptionConstants.PARSER_INVALID_FACTORY_ELEMENT.Format(terminalType, typeof(ITerminalNodeFactory)));

            // get collection method on factory using reflection
            MethodInfo registryMethod = terminalType.GetMethod(nameof(ITerminalNodeFactory.GetTerminals))!;
            
            // invoke and iterate to register
            IEnumerable<TerminalDefinition> definitions = (IEnumerable<TerminalDefinition>)registryMethod.Invoke(null, Array.Empty<object>())!;
            foreach (TerminalDefinition definition in definitions)
            {
                m_terminalDefinitions.Add(definition.Name, definition);
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
                m_nonTerminalDefinitions.Add(definition.Name, definition);
            }
        }
    }

    public IEnumerable<TerminalDefinition> GetTerminalDefinitions(string name)
    {
        if (m_terminalDefinitions.TryGetValue(name, out List<TerminalDefinition>? definitions))
        {
            return definitions;
        }

        throw new ParserDefinitionException(ExceptionConstants.PARSER_UNKOWN_TERMINAL.Format(name));
    }
    
    public IEnumerable<NonTerminalDefinition> GetNonTerminalDefinitions(string name)
    {
        if (m_nonTerminalDefinitions.TryGetValue(name, out List<NonTerminalDefinition>? definitions))
        {
            return definitions;
        }

        throw new ParserDefinitionException(ExceptionConstants.PARSER_UNKOWN_NONTERMINAL.Format(name));
    }
}