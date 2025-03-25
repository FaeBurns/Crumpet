using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class IfStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public ExpressionNode Expression { get; }
    public IInstructionProvider TrueBody { get; }
    public IInstructionProvider? FalseBody { get; }

    public IfStatementNode(ExpressionNode expression, NonTerminalNode trueBody, NonTerminalNode? falseBody) : base(expression, trueBody, falseBody)
    {
        Expression = expression;
        TrueBody = (IInstructionProvider)trueBody;
        FalseBody = (IInstructionProvider)falseBody;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // { } variant
        yield return new NonTerminalDefinition<IfStatementNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_IF),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new NonTerminalConstraint<ExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new OrConstraint(new NonTerminalConstraint<StatementBodyNode>(), new NonTerminalConstraint<StatementNode>()),
                new OptionalConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.KW_ELSE),
                    new OrConstraint(new NonTerminalConstraint<StatementBodyNode>(), new NonTerminalConstraint<StatementNode>())))),
            GetNodeConstructor<IfStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
        yield return new ConditionalExecutionInstruction(
            new InstructionCollator(TrueBody),
            new InstructionCollator(FalseBody), 
            Location);
    }
}