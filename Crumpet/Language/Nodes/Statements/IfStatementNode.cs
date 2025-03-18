using Crumpet.Instructions.Flow;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;
using Shared;

namespace Crumpet.Language.Nodes.Statements;

public class IfStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
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
        yield return new NonTerminalDefinition<IfStatementNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_IF),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new NonTerminalConstraint<ExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>(),
                new OptionalConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.KW_ELSE),
                    new NonTerminalConstraint<StatementBodyNode>()))),
            GetNodeConstructor<IfStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return new ConditionalExecutionInstruction(Location, 
            new InstructionCollator(TrueBody),
            new InstructionCollator(FalseBody));
    }
}