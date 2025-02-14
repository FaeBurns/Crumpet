using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Expressions;

public class EqualityExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public RelationExpressionNode Primary { get; }
    public RawKeywordNode Sugar { get; }
    public RelationExpressionNode? Secondary { get; }

    public EqualityExpressionNode(RelationExpressionNode primary, RawKeywordNode sugar, RelationExpressionNode? secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("equalityExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("relationExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new OrConstraint(
                            new RawTerminalConstraint("==", true),
                            new RawTerminalConstraint("!=", true)),
                        new NonTerminalConstraint("relationExpression")))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}