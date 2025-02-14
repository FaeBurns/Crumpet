using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Expressions;

public class MultExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public UnaryExpressionNode Primary { get; }
    public RawKeywordNode Sugar { get; }
    public UnaryExpressionNode Secondary { get; }

    public MultExpressionNode(UnaryExpressionNode primary, RawKeywordNode sugar, UnaryExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("multExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("unaryExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new RawTerminalConstraint("*", true),
                            new RawTerminalConstraint("/", true)),
                        new NonTerminalConstraint("unaryExpression")))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}