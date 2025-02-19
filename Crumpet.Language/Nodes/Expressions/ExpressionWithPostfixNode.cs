using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Expressions;

public abstract class ExpressionWithPostfixNode : NonTerminalNode, INonTerminalNodeFactory
{
    public PrimaryExpressionNode Expression { get; }

    protected ExpressionWithPostfixNode(PrimaryExpressionNode expression, params IEnumerable<ASTNode?> childRefs) : base(childRefs.Prepend(expression))
    {
        Expression = expression;
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
            GetNodeConstructor<ExpressionWithPostfixNodeIndexVariant>());
        
        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OptionalConstraint(new NonTerminalConstraint<ArgumentExpressionListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN)),
            GetNodeConstructor<ExpressionWithPostfixNodeParansVariant>());

        yield return new NonTerminalDefinition<ExpressionWithPostfixNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<PrimaryExpressionNode>(),
                new OneOrMoreConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.PERIOD),
                        new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)))),
            GetNodeConstructor<ExpressionWithPostfixNodeIdentifierVariant>());
    }
}

public class ExpressionWithPostfixNodeIndexVariant : ExpressionWithPostfixNode
{
    public ArgumentExpressionListNode Arguments { get; }

    public ExpressionWithPostfixNodeIndexVariant(PrimaryExpressionNode expression, ArgumentExpressionListNode arguments) : base(expression, arguments)
    {
        Arguments = arguments;
    }
}

public class ExpressionWithPostfixNodeParansVariant : ExpressionWithPostfixNode
{
    public ArgumentExpressionListNode? Arguments { get; }

    public ExpressionWithPostfixNodeParansVariant(PrimaryExpressionNode expression, ArgumentExpressionListNode? arguments) : base(expression, arguments)
    {
        Arguments = arguments;
    }
}

public class ExpressionWithPostfixNodeIdentifierVariant : ExpressionWithPostfixNode
{
    public string FullExpressionIdentifier { get; }
    public IdentifierNode[] IdentifierSections { get; }
    
    // ReSharper disable PossibleMultipleEnumeration
    public ExpressionWithPostfixNodeIdentifierVariant(PrimaryExpressionNode expression, IEnumerable<IdentifierNode> identifierList) : base(expression, identifierList)
    {
        IdentifierSections = identifierList.ToArray();
        FullExpressionIdentifier = string.Join('.', IdentifierSections.Select(s => s.ToString()));
    }
}