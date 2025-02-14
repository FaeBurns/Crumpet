using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

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
        yield return new NonTerminalDefinition("exclusiveOrExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("equalityExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new RawTerminalConstraint("^"),
                        new NonTerminalConstraint("equalityExpression")))),
            GetNodeConstructor<ExclusiveOrExpressionNode>());
    }
}