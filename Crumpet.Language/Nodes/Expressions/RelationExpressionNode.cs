using Crumpet.Language.Nodes.Constraints;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class RelationExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public SumExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public SumExpressionNode? Secondary { get; }

    public RelationExpressionNode(SumExpressionNode primary, TerminalNode<CrumpetToken> sugar, SumExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public RelationExpressionNode(SumExpressionNode primary) : base(primary)
    {
        Primary = primary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // relation
        yield return new NonTerminalDefinition<RelationExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<SumExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.LESS),
                        new CrumpetTerminalConstraint(CrumpetToken.LESS_OR_EQUAL),
                        new CrumpetTerminalConstraint(CrumpetToken.GREATER_OR_EQUAL),
                        new CrumpetTerminalConstraint(CrumpetToken.GREATER)),
                    new NonTerminalConstraint<SumExpressionNode>())),
            GetNodeConstructor<RelationExpressionNode>(3));
        
        // passthrough
        yield return new NonTerminalDefinition<RelationExpressionNode>(
                new NonTerminalConstraint<SumExpressionNode>(),
            GetNodeConstructor<RelationExpressionNode>(1));
    }
}