using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public abstract class AssignmentExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected AssignmentExpressionNode(params IEnumerable<ASTNode> implicitChildren) : base(implicitChildren)
    {
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<AssignmentExpressionNode>(new NonTerminalConstraint<OrExpressionNode>(), GetNodeConstructor<AssignmentExpressionNodePassthroughVariant>());

        yield return new NonTerminalDefinition<AssignmentExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<UnaryExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.EQUALS),
                new NonTerminalConstraint<AssignmentExpressionNode>()
            ), GetNodeConstructor<AssignmentExpressionNodeAssignmentVariant>());
    }
}

public class AssignmentExpressionNodePassthroughVariant : AssignmentExpressionNode
{
    public OrExpressionNode OrExpression { get; }
    
    public AssignmentExpressionNodePassthroughVariant(OrExpressionNode orExpression) : base(orExpression)
    {
        OrExpression = orExpression;
    }
}

public class AssignmentExpressionNodeAssignmentVariant : AssignmentExpressionNode
{
    public UnaryExpressionNode UnaryExpression { get; }
    public AssignmentExpressionNode AssignmentExpression { get; }
    
    public AssignmentExpressionNodeAssignmentVariant(UnaryExpressionNode unaryExpression, AssignmentExpressionNode assignmentExpression) : base(unaryExpression, assignmentExpression)
    {
        UnaryExpression = unaryExpression;
        AssignmentExpression = assignmentExpression;
    }
}