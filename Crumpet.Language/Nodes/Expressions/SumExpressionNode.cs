using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
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
        yield return new NonTerminalDefinition("sumExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("multExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new RawTerminalConstraint("+", true),
                            new RawTerminalConstraint("-", true)),
                        new NonTerminalConstraint("multExpression")))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}