using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public class MultExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public UnaryExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken> Sugar { get; }
    public UnaryExpressionNode Secondary { get; }

    public MultExpressionNode(UnaryExpressionNode primary, TerminalNode<CrumpetToken> sugar, UnaryExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<MultExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<UnaryExpressionNode>(),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY, true),
                            new CrumpetTerminalConstraint(CrumpetToken.DIVIDE, true)),
                        new NonTerminalConstraint<UnaryExpressionNode>()))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}