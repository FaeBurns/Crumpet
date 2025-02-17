using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;

namespace Crumpet.Language.Nodes.Statements;

public class IterationStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExpressionNode Expression { get; }
    public StatementBodyNode Body { get; }

    public IterationStatementNode(ExpressionNode expression, StatementBodyNode body) : base(expression, body)
    {
        Expression = expression;
        Body = body;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<IterationStatementNode>( 
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_WHILE),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new NonTerminalConstraint<ExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<IterationStatementNode>());
    }
}