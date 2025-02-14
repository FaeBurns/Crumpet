using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
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
        yield return new NonTerminalDefinition("relationExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("sumExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new RawTerminalConstraint("<", true),
                            new RawTerminalConstraint("<=", true),
                            new RawTerminalConstraint(">=", true),
                            new RawTerminalConstraint(">", true)),
                        new NonTerminalConstraint("sumExpression")))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}