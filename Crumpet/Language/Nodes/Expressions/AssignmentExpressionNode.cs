using System.Collections;
using Crumpet.Instructions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public abstract class AssignmentExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected AssignmentExpressionNode(params IEnumerable<ASTNode?> implicitChildren) : base(implicitChildren)
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

public class AssignmentExpressionNodePassthroughVariant : AssignmentExpressionNode, IInstructionProvider
{
    public OrExpressionNode OrExpression { get; }

    public AssignmentExpressionNodePassthroughVariant(OrExpressionNode orExpression) : base(orExpression)
    {
        OrExpression = orExpression;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return OrExpression;
    }
}

public class AssignmentExpressionNodeAssignmentVariant : AssignmentExpressionNode, IInstructionProvider
{
    public UnaryExpressionNode UnaryExpression { get; }
    public AssignmentExpressionNode AssignmentExpression { get; }


    public AssignmentExpressionNodeAssignmentVariant(UnaryExpressionNode unaryExpression, AssignmentExpressionNode assignmentExpression) : base(unaryExpression, assignmentExpression)
    {
        UnaryExpression = unaryExpression;
        AssignmentExpression = assignmentExpression;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return UnaryExpression;
        yield return AssignmentExpression;
        yield return new AssignVariableInstruction(Location);
    }
}