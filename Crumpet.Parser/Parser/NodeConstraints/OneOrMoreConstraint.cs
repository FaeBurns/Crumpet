using Crumpet.Parser.Elements;
using Crumpet.Parser.Nodes;

namespace Crumpet.Parser.NodeConstraints;

public class OneOrMoreConstraint : ContainsSingleConstraint
{
    public OneOrMoreConstraint(NodeConstraint constraint) : base(constraint)
    {
    }

    public override string ToString()
    {
        return Constraint.ToString() + "+";
    }

    public override ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        List<ParserElement> childElements = new List<ParserElement>();

        int originalPosition = stream.Position;
        
        while (true)
        {
            int position = stream.Position;
            ParserElement? element = Constraint.WalkStream(stream, registry);
            
            // if element was not found, return all that have been
            if (element == null)
            {
                stream.Position = position;
                break;
            }
            
            childElements.Add(element);
        }

        // if there is at least one
        if (childElements.Count > 0)
        {
            // return a collection of all found elements
            return new MultipleParserElements(childElements, false);
        }
        
        // otherwise reset the stream position and return null
        stream.Position = originalPosition;
        return null;
    }
}