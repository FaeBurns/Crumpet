using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Expressions;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Statements;

public class FlowStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public RawKeywordNode Keyword { get; }
    public ExpressionNode? Expression { get; }

    public FlowStatementNode(RawKeywordNode keyword, ExpressionNode? expression) : base(keyword, expression)
    {
        Keyword = keyword;
        Expression = expression;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("flowStatement",
            new SequenceConstraint(
                new OrConstraint(
                    new RawTerminalConstraint("continue", true),
                    new RawTerminalConstraint("break", true),
                    new SequenceConstraint(
                        new RawTerminalConstraint("return", true),
                        new OptionalConstraint(
                            new NonTerminalConstraint("expression")))),
                new RawTerminalConstraint(";")), 
            GetNodeConstructor<FlowStatementNode>());
    }
}