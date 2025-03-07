using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class PrimaryExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ASTNode Variant { get; }

    protected PrimaryExpressionNode(ASTNode variant)
    {
        Variant = variant;
    }
    
    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        yield return Variant;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER), GetNodeConstructor<PrimaryExpressionNodeIdentifierVariant>());
        
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new NonTerminalConstraint<LiteralConstantNode>(), GetNodeConstructor<PrimaryExpressionNodeLiteralConstantVariant>());
        
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new SequenceConstraint(
            new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
            new NonTerminalConstraint<ExpressionNode>(),
            new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN)
            ),
            GetNodeConstructor<PrimaryExpressionNodeNestedExpressionVariant>());
    }
}

public class PrimaryExpressionNodeIdentifierVariant : PrimaryExpressionNode
{
    public IdentifierNode Identifier { get; }

    public PrimaryExpressionNodeIdentifierVariant(IdentifierNode identifier) : base(identifier)
    {
        Identifier = identifier;
    }
}

public class PrimaryExpressionNodeLiteralConstantVariant : PrimaryExpressionNode
{
    public LiteralConstantNode LiteralConstant { get; }

    public PrimaryExpressionNodeLiteralConstantVariant(LiteralConstantNode literalConstant) : base(literalConstant)
    {
        LiteralConstant = literalConstant;
    }
}

public class PrimaryExpressionNodeNestedExpressionVariant : PrimaryExpressionNode
{
    public ExpressionNode Expression { get; }

    public PrimaryExpressionNodeNestedExpressionVariant(ExpressionNode expression) : base(expression)
    {
        Expression = expression;
    }
}
