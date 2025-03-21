using System.Linq.Expressions;
using Crumpet.Exceptions;
using Crumpet.Instructions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;
using Shared;

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
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                    new NonTerminalConstraint<ExpressionNode>(),
                    new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX))),
            GetNodeConstructor<ExpressionWithPostfixNodeIndexVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OptionalConstraint(new NonTerminalConstraint<ArgumentExpressionListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN)), GetNodeConstructor<ExpressionWithPostfixNodeExecutionVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new OneOrMoreConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.PERIOD),
                        new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)))),
            GetNodeConstructor<ExpressionWithPostfixNodeIdentifierVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.PLUSPLUS),
                    new CrumpetTerminalConstraint(CrumpetToken.MINUSMINUS))),
            GetNodeConstructor<ExpressionWithPostfixNodeIncrementVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new NonTerminalConstraint<PrimaryExpressionNode>(),
            GetNodeConstructor<ExpressionWithPostfixNodePassthroughVariant>());
    }
}

public class ExpressionWithPostfixNodeIncrementVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public PrimaryExpressionNode Expression { get; }
    public TerminalNode<CrumpetToken> Sugar { get; }

    public ExpressionWithPostfixNodeIncrementVariant(PrimaryExpressionNode expression, TerminalNode<CrumpetToken> sugar) : base(expression, sugar)
    {
        Expression = expression;
        Sugar = sugar;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return new IncrementInstruction(Sugar.Token.TokenId == CrumpetToken.PLUSPLUS);
    }
}

public class ExpressionWithPostfixNodeExecutionVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public IdentifierNode Identifier { get; }
    public ArgumentExpressionListNode? Arguments { get; }

    public ExpressionWithPostfixNodeExecutionVariant(IdentifierNode identifier, ArgumentExpressionListNode? arguments) : base(identifier, arguments)
    {
        Identifier = identifier;
        Arguments = arguments;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        // evaluate all args
        yield return Arguments;
        // execute
        yield return new ExecuteFunctionInstruction(Identifier.Terminal, Arguments?.Expressions.Length ?? 0);
    }
}

public class ExpressionWithPostfixNodeIndexVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public PrimaryExpressionNode Expression { get; }
    public ExpressionNode Argument { get; }

    public ExpressionWithPostfixNodeIndexVariant(PrimaryExpressionNode expression, ExpressionNode argument) : base(expression, argument)
    {
        Expression = expression;
        Argument = argument;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return Argument;
        yield return new AccessIndexInstruction();
    }
}

public class ExpressionWithPostfixNodeIdentifierVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public PrimaryExpressionNode Expression { get; }
    public string FullExpressionIdentifier { get; }
    public IdentifierNode[] IdentifierSections { get; }

    // ReSharper disable PossibleMultipleEnumeration
    public ExpressionWithPostfixNodeIdentifierVariant(PrimaryExpressionNode expression, IEnumerable<IdentifierNode> identifierList) : base(identifierList.Prepend<ASTNode?>(expression))
    {
        Expression = expression;
        IdentifierSections = identifierList.ToArray();
        FullExpressionIdentifier = String.Join('.', IdentifierSections.Select(s => s.ToString()));
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return new PopAndSearchField(IdentifierSections.Select(s => s.Terminal));
    }
}

public class ExpressionWithPostfixNodePassthroughVariant : ExpressionWithPostfixNode, IInstructionProvider
{
    public PrimaryExpressionNode PrimaryExpression { get; }

    public ExpressionWithPostfixNodePassthroughVariant(PrimaryExpressionNode primaryExpression) : base(primaryExpression)
    {
        PrimaryExpression = primaryExpression;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return PrimaryExpression;
    }
}
