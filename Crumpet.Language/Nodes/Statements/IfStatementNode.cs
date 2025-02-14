using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Expressions;

namespace Crumpet.Language.Nodes.Statements;

public class IfStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ExpressionNode Expression { get; }
    public StatementBodyNode TrueBody { get; }
    public StatementBodyNode? FalseBody { get; }

    public IfStatementNode(ExpressionNode expression, StatementBodyNode trueBody, StatementBodyNode? falseBody) : base(expression, trueBody, falseBody)
    {
        Expression = expression;
        TrueBody = trueBody;
        FalseBody = falseBody;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("ifStatement",
            new SequenceConstraint(
                new RawTerminalConstraint("if"),
                new RawTerminalConstraint("("),
                new NonTerminalConstraint("expression"),
                new RawTerminalConstraint(")"),
                new NonTerminalConstraint("statementBody"),
                new OptionalConstraint(new SequenceConstraint(
                    new RawTerminalConstraint("else"),
                    new NonTerminalConstraint("statementBody")))),
            GetNodeConstructor<IfStatementNode>());
    }
}