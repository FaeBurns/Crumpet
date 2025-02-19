using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public class RelationExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public SumExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public SumExpressionNode? Secondary { get; }

    public RelationExpressionNode(SumExpressionNode primary, (TerminalNode<CrumpetToken> sugar, SumExpressionNode secondary)? optional) : base(primary, optional?.sugar, optional?.secondary)
    {
        Primary = primary;
        Sugar = optional?.sugar;
        Secondary = optional?.secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<RelationExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<SumExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new CrumpetTerminalConstraint(CrumpetToken.LESS),
                            new CrumpetTerminalConstraint(CrumpetToken.LESS_OR_EQUAL),
                            new CrumpetTerminalConstraint(CrumpetToken.GREATER_OR_EQUAL),
                            new CrumpetTerminalConstraint(CrumpetToken.GREATER)),
                        new NonTerminalConstraint<SumExpressionNode>()))),
            GetNodeConstructor<RelationExpressionNode>());
    }
}