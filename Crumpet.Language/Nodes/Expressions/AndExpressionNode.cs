using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class AndExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExclusiveOrExpressionNode Primary { get; }
    public ExclusiveOrExpressionNode? Secondary { get; }

    public AndExpressionNode(ExclusiveOrExpressionNode primary, ExclusiveOrExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("andExpression", 
            new SequenceConstraint(
                new NonTerminalConstraint("exclusiveOrExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new RawTerminalConstraint("&&"),
                        new NonTerminalConstraint("exclusiveOrExpression")))),
            GetNodeConstructor<AndExpressionNode>());
    }
}