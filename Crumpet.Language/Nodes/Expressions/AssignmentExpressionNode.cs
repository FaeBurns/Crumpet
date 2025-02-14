using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public abstract class AssignmentExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected AssignmentExpressionNode(params IEnumerable<ASTNode> implicitChildren) : base(implicitChildren)
    {
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("assignmentExpression", new NonTerminalConstraint("orExpression"), GetNodeConstructor<AssignmentExpressionNodePassthroughVariant>());

        yield return new NonTerminalDefinition("assignmentExpression",
            new SequenceConstraint(
                new NonTerminalConstraint("unaryExpression"),
                new RawTerminalConstraint("="),
                new NonTerminalConstraint("assignmentExpression")
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