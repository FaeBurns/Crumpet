using Parser.Elements;
using Parser.Nodes;

namespace Parser.NodeConstraints;

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

        // collection of definition and the amount of tokens it consumed
        List<NonTerminalWalkResult> largestDefinitions = new List<NonTerminalWalkResult>();

        foreach (NonTerminalDefinition definition in definitionRules)
        {
            int position = stream.Position;
            NonTerminalInstanceConstructor<T> constructor = new NonTerminalInstanceConstructor<T>(definition);
            ASTNode? node = constructor.Construct(stream, registry);
            
            int length = stream.Position - position;
            
            // only collect rule if node was valid
            // record stream.Position as that one is the position as of now - after the node was created
            if (node is not null)
                largestDefinitions.Add(new NonTerminalWalkResult(stream.Position, length, node));
            
            stream.Position = position;
        }

        NonTerminalWalkResult? largestDefinition = largestDefinitions.OrderByDescending(r => r.Length).FirstOrDefault();

        // if null then collection was empty - return no node
        if (largestDefinition is null)
            return null;
        
        // restore position to what it was after constructing the node
        stream.Position = largestDefinition.Position;
        
        // return the largest node
        return largestDefinition.Node;
    }

    private class NonTerminalWalkResult
    {
        public int Position { get; }
        public int Length { get; }
        public ASTNode Node { get; }

        public NonTerminalWalkResult(int position, int length, ASTNode node)
        {
            Position = position;
            Length = length;
            Node = node;
        }
    }
}

public class NonTerminalConstraint<T> : NonTerminalConstraint
{
    public NonTerminalConstraint() : base(typeof(T)) { }
}