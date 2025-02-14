using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class OrExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public AndExpressionNode Primary { get; }
    public AndExpressionNode? Secondary { get; }

    public OrExpressionNode(AndExpressionNode primary, AndExpressionNode? secondary) : base(primary, secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("orExpression", 
            new SequenceConstraint(
                new NonTerminalConstraint("andExpression"),
                new OptionalConstraint(
                    new SequenceConstraint(
                        new RawTerminalConstraint("||"),
                        new NonTerminalConstraint("andExpression")))),
            GetNodeConstructor<OrExpressionNode>());
    }
}