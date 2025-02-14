using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class TypeNode : NonTerminalNode, INonTerminalNodeFactory
{
    public string FullName { get; }
    public IdentifierNode[] Segments { get; }
    
    public TypeNode(IdentifierNode firstSegment, IEnumerable<IdentifierNode> otherSegments)
    {
        // prevent double enumeration
        IEnumerable<IdentifierNode> identifierNodes = otherSegments as IReadOnlyList<IdentifierNode> ?? otherSegments.ToArray();
        
        // creates a temporary iterator with firstSegment at the start
        Segments = identifierNodes.Prepend(firstSegment).ToArray();

        // set full name as first.other1.other2
        FullName = firstSegment.Terminal + String.Concat(identifierNodes.Select(s => '.' + s.Terminal));
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("type",
            new SequenceConstraint(
                new NamedTerminalConstraint("identifier"),
                new ZeroOrMoreConstraint(new SequenceConstraint(
                    new RawTerminalConstraint("\\."),
                    new NamedTerminalConstraint("identifier")))),
            GetNodeConstructor<TypeNode>());
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (IdentifierNode node in Segments)
        {
            yield return node;
        }
    }
}