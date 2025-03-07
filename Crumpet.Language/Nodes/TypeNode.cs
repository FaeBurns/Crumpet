using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes;

public abstract class TypeNode : NonTerminalNode, INonTerminalNodeFactory
{
    public required string FullName { get; init; }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new ZeroOrMoreConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.PERIOD),
                    new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)))),
            GetNodeConstructor<TypeNodeIdentifierVariant>());

        yield return new NonTerminalDefinition<TypeNode>(
            new CrumpetTerminalConstraint(CrumpetToken.KW_KNOWN_TYPE), 
            GetNodeConstructor<TypeNodeKeywordVariant>());
    }

    public override string ToString()
    {
        return FullName;
    }
}

public class TypeNodeIdentifierVariant : TypeNode
{
    public IdentifierNode[] Segments { get; }
    
    public TypeNodeIdentifierVariant(IdentifierNode firstSegment, IEnumerable<IdentifierNode> otherSegments)
    {
        // prevent double enumeration
        IEnumerable<IdentifierNode> identifierNodes = otherSegments as IReadOnlyList<IdentifierNode> ?? otherSegments.ToArray();
        
        // creates a temporary iterator with firstSegment at the start
        Segments = identifierNodes.Prepend(firstSegment).ToArray();

        // set full name as first.other1.other2
        FullName = firstSegment.Terminal + String.Concat(identifierNodes.Select(s => '.' + s.Terminal));
    }
    
    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (IdentifierNode node in Segments)
        {
            yield return node;
        }
    }
}

public class TypeNodeKeywordVariant : TypeNode
{
    public TerminalNode<CrumpetToken> Keyword { get; }
    
    public TypeNodeKeywordVariant(TerminalNode<CrumpetToken> keyword)
    {
        Keyword = keyword;
        FullName = keyword.Terminal;
    }

    protected override IEnumerable<ASTNode?> EnumerateChildrenDerived()
    {
        yield return Keyword;
    }
}