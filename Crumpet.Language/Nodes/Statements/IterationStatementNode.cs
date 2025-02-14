using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
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
        yield return new NonTerminalDefinition("iterationStatement", 
            new SequenceConstraint(
                new RawTerminalConstraint("while"),
                new RawTerminalConstraint("("),
                new NonTerminalConstraint("expression"),
                new RawTerminalConstraint(")"),
                new NonTerminalConstraint("statementBody")),
            GetNodeConstructor<IterationStatementNode>());
    }
}