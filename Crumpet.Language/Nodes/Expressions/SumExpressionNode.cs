using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public class SumExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public MultExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public MultExpressionNode? Secondary { get; }

    public SumExpressionNode(MultExpressionNode primary, (TerminalNode<CrumpetToken> sugar, MultExpressionNode secondary)? optional) : base(primary, optional?.sugar, optional?.secondary)
    {
        Primary = primary;
        Sugar = optional?.sugar;
        Secondary = optional?.secondary;
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
            GetNodeConstructor<SumExpressionNode>());
    }
}