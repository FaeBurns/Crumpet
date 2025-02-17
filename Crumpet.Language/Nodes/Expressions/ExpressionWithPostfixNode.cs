using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public class ExpressionWithPostfixNode : NonTerminalNode, INonTerminalNodeFactory
{
    public PrimaryExpressionNode Expression { get; }
    public ArgumentExpressionListNode? Arguments { get; }

    public ExpressionWithPostfixNode(PrimaryExpressionNode expression, ArgumentExpressionListNode? arguments)
    {
        Expression = expression;
        Arguments = arguments;
    }
    
    protected override IEnumerable<ASTNode?> EnumerateChildrenDerived()
    {
        yield return Expression;
        yield return Arguments;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new OptionalConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                    new NonTerminalConstraint<ArgumentExpressionListNode>(),
                    new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX)))),
            GetNodeConstructor<ExpressionWithPostfixNode>());
    }
}