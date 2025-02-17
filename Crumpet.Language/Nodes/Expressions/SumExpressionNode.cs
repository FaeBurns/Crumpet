using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Expressions;

public class SumExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public MultExpressionNode Primary { get; }
    public RawKeywordNode Sugar { get; }
    public MultExpressionNode Secondary { get; }

    public SumExpressionNode(MultExpressionNode primary, RawKeywordNode sugar, MultExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<SumExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<MultExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new CrumpetTerminalConstraint(CrumpetToken.PLUS),
                            new CrumpetTerminalConstraint(CrumpetToken.MINUS)),
                        new NonTerminalConstraint<MultExpressionNode>()))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}