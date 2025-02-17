using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Expressions;

public class RelationExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public SumExpressionNode Primary { get; }
    public RawKeywordNode Sugar { get; }
    public SumExpressionNode? Secondary { get; }

    public RelationExpressionNode(SumExpressionNode primary, RawKeywordNode sugar, SumExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
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
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}