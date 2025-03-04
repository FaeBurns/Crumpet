using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class SequenceConstraint : MultiNodeConstraint
{
    public SequenceConstraint(params IEnumerable<NodeConstraint> constraints) : base(constraints)
    {
    }

    public override string ToString()
    {
        return string.Join(' ', Constraints.Select(c => c.ToString()));
    }

    public override ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        int position = stream.Position;
        List<ParserElement> elements = new List<ParserElement>();
        foreach (NodeConstraint constraint in Constraints)
        {
            ParserElement? element = constraint.WalkStream(stream, registry);
            if (element is null)
            {
                // reset position and return null
                stream.Position = position;
                return null;
            }
            
            // if element is an empty collection then also reset the position
            // if (element is MultipleParserElements multiElement && !multiElement.Collection.Any())
                // stream.Position = position;
            
            // if the element was valid then record it
            elements.Add(element);
        }

        // if the loop ends then all elements were valid
        // return all recorded elements
        return new MultipleParserElements(elements, true);
    }
}