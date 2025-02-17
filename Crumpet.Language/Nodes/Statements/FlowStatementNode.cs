using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
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
        yield return new NonTerminalDefinition<FlowStatementNode>(
            new SequenceConstraint(
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.KW_CONTINUE),
                    new CrumpetTerminalConstraint(CrumpetToken.KW_BREAK),
                    new SequenceConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.KW_RETURN),
                        new OptionalConstraint(
                            new NonTerminalConstraint<ExpressionNode>()))),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)), 
            GetNodeConstructor<FlowStatementNode>());
    }
}