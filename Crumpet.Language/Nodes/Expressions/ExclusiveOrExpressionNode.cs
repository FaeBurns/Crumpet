using Crumpet.Language.Nodes.Constraints;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class ExclusiveOrExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public EqualityExpressionNode Primary { get; }
    public EqualityExpressionNode? Secondary { get; }

    public ExclusiveOrExpressionNode(EqualityExpressionNode primary, EqualityExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExclusiveOrExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<EqualityExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.XOR),
                        new NonTerminalConstraint<EqualityExpressionNode>()))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}