using Crumpet.Instructions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public abstract class ExpressionWithPostfixNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected ExpressionWithPostfixNode(params IEnumerable<ASTNode?> children) : base(children)
    {
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ExpressionWithPointerPrefixNode>(),
                new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                    new NonTerminalConstraint<ExpressionNode>(),
                    new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX))),
            GetNodeConstructor<ExpressionWithPostfixNodeIndexVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new OptionalConstraint(new NonTerminalConstraint<TypeArgumentListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OptionalConstraint(new NonTerminalConstraint<ArgumentExpressionListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN)), GetNodeConstructor<ExpressionWithPostfixNodeExecutionVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ExpressionWithPointerPrefixNode>(),
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.PLUSPLUS),
                    new CrumpetTerminalConstraint(CrumpetToken.MINUSMINUS))),
            GetNodeConstructor<ExpressionWithPostfixNodeIncrementVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new NonTerminalConstraint<ExpressionWithPointerPrefixNode>(),
            GetNodeConstructor<ExpressionWithPostfixNodePassthroughVariant>());
    }
}

public class ExpressionWithPostfixNodeIncrementVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public ExpressionWithPointerPrefixNode Expression { get; }
    public TerminalNode<CrumpetToken> Sugar { get; }

    public ExpressionWithPostfixNodeIncrementVariant(ExpressionWithPointerPrefixNode expression, TerminalNode<CrumpetToken> sugar) : base(expression, sugar)
    {
        Expression = expression;
        Sugar = sugar;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return new IncrementInstruction(Sugar.Token.TokenId == CrumpetToken.PLUSPLUS, Location);
    }
}

public class ExpressionWithPostfixNodeExecutionVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public IdentifierNode Identifier { get; }
    public TypeArgumentListNode TypeArgs { get; }
    public ArgumentExpressionListNode? Arguments { get; }

    public ExpressionWithPostfixNodeExecutionVariant(IdentifierNode identifier, TypeArgumentListNode typeArgs, ArgumentExpressionListNode? arguments) : base(identifier, typeArgs, arguments)
    {
        Identifier = identifier;
        TypeArgs = typeArgs;
        Arguments = arguments;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        // evaluate all args
        yield return Arguments;

        // evaluate all type args
        yield return TypeArgs;
        
        // execute
        yield return new ExecuteFunctionInstruction(Identifier.Terminal, TypeArgs?.Types.Length ?? 0, Arguments?.Expressions.Length ?? 0, Location);
    }
}

public class ExpressionWithPostfixNodeIndexVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public ExpressionWithPointerPrefixNode Expression { get; }
    public ExpressionNode Argument { get; }

    public ExpressionWithPostfixNodeIndexVariant(ExpressionWithPointerPrefixNode expression, ExpressionNode argument) : base(expression, argument)
    {
        Expression = expression;
        Argument = argument;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return Argument;
        yield return new AccessIndexInstruction(Location);
    }
}

public class ExpressionWithPostfixNodePassthroughVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public ExpressionWithPointerPrefixNode PrimaryExpression { get; }

    public ExpressionWithPostfixNodePassthroughVariant(ExpressionWithPointerPrefixNode primaryExpression) : base(primaryExpression)
    {
        PrimaryExpression = primaryExpression;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return PrimaryExpression;
    }
}
