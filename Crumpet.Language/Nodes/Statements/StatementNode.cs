using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Expressions;

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
        yield return new NonTerminalDefinition("statement", 
            new SequenceConstraint(
                new OptionalConstraint(new NonTerminalConstraint("expression")),
                new RawTerminalConstraint(";")),
            GetNodeConstructor<StatementNodeExpressionVariant>());

        yield return new NonTerminalDefinition("statement",
            new NonTerminalConstraint("ifStatement"), 
            GetNodeConstructor<StatementNodeIfVariant>());
        
        yield return new NonTerminalDefinition("statement",
            new NonTerminalConstraint("iterationStatement"), 
            GetNodeConstructor<StatementNodeIterationVariant>());
        
        yield return new NonTerminalDefinition("statement",
            new NonTerminalConstraint("flowStatement"), 
            GetNodeConstructor<StatementNodeFlowVariant>());
    }
}

public class StatementNodeExpressionVariant : StatementNode
{
    public ExpressionNode? Expression { get; }
    
    public StatementNodeExpressionVariant(ExpressionNode? expression) : base(expression)
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