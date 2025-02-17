using System.Reflection;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class NonTerminalConstraint : NodeConstraint
{
    public Type NonTerminalType { get; }

    protected NonTerminalConstraint(Type nonTerminalType)
    {
        NonTerminalType = nonTerminalType;
    }

    public override string ToString()
    {
        return NonTerminalType.Name.Replace("Node", String.Empty);
    }

    public override ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        
        IEnumerable<NonTerminalDefinition> definitionRules = registry.GetNonTerminalDefinitions(NonTerminalType);

        foreach (NonTerminalDefinition definition in definitionRules)
        {
            NonTerminalInstanceConstructor<T> constructor = new NonTerminalInstanceConstructor<T>(definition);
            ASTNode? node = constructor.Construct(stream, registry);

            if (node is not null)
                return node;
        }

        // no rules passed, return no node
        return null;
    }
}

public class NonTerminalConstraint<T> : NonTerminalConstraint
{
    public NonTerminalConstraint() : base(typeof(T)) { }
}