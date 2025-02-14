using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

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
        yield return new NonTerminalDefinition("primaryExpression", new NamedTerminalConstraint("identifier"), GetNodeConstructor<PrimaryExpressionNodeIdentifierVariant>());
        
        yield return new NonTerminalDefinition("primaryExpression", new NonTerminalConstraint("literalConstant"), GetNodeConstructor<PrimaryExpressionNodeLiteralConstantVariant>());
        
        yield return new NonTerminalDefinition("primaryExpression", new SequenceConstraint(
            new RawTerminalConstraint("("),
            new NonTerminalConstraint("expression"),
            new RawTerminalConstraint(")")
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
