using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class StatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ASTNode? VariantNode { get; }

    protected StatementNode(ASTNode? variantNode) : base(variantNode!)
    {
        VariantNode = variantNode;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<StatementNode>(
            new SequenceConstraint(
                new OptionalConstraint(new NonTerminalConstraint<ExpressionNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<StatementNodeExpressionVariant>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<IfStatementNode>(),
            GetNodeConstructor<StatementNodeIfVariant>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<IterationStatementNode>(),
            GetNodeConstructor<StatementNodeIterationVariant>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<FlowStatementNode>(),
            GetNodeConstructor<StatementNodeFlowVariant>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<InitializationStatementNode>(),
            GetNodeConstructor<StatementNodeInitializationVariant>());
    }
}

public class StatementNodeExpressionVariant : StatementNode
{
    public ExpressionNode? Expression { get; }

    protected StatementNodeExpressionVariant(ExpressionNode? expression) : base(expression)
    {
        Expression = expression;
    }
}

public class StatementNodeIfVariant : StatementNode
{
    public IfStatementNode IfStatement { get; }

    protected StatementNodeIfVariant(IfStatementNode ifStatement) : base(ifStatement)
    {
        IfStatement = ifStatement;
    }
}

public class StatementNodeIterationVariant : StatementNode
{
    public IterationStatementNode IterationStatement { get; }

    protected StatementNodeIterationVariant(IterationStatementNode iterationStatement) : base(iterationStatement)
    {
        IterationStatement = iterationStatement;
    }
}

public class StatementNodeFlowVariant : StatementNode
{
    public FlowStatementNode FlowStatement { get; }

    protected StatementNodeFlowVariant(FlowStatementNode flowStatement) : base(flowStatement)
    {
        FlowStatement = flowStatement;
    }
}

public class StatementNodeInitializationVariant : StatementNode
{
    public InitializationStatementNode InitializationStatement { get; }

    protected StatementNodeInitializationVariant(InitializationStatementNode initializationStatement) : base(initializationStatement)
    {
        InitializationStatement = initializationStatement;
    }
}