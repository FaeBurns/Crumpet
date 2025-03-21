using Crumpet.Instructions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class WhileStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public ExpressionNode Expression { get; }
    public StatementBodyNode Body { get; }

    public WhileStatementNode(ExpressionNode expression, StatementBodyNode body) : base(expression, body)
    {
        Expression = expression;
        Body = body;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<WhileStatementNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_WHILE),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new NonTerminalConstraint<ExpressionNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<WhileStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        // hold everything inside it's own executable unit - helps make label searching easier during execution
        yield return new ExecuteUnitInstruction(Location, new InstructionCollator(GetLoopInstructions()));
    }

    private IEnumerable GetLoopInstructions()
    {
        // Flow:
        // check condition
        // if condition passes keep going
        // if the condition fails jump past the body
        // jump back to the condition 
        
        Guid conditionLabel = Guid.NewGuid();
        Guid exitLabel = Guid.NewGuid();
        
        // start of loop label
        yield return new LabelInstruction(conditionLabel);
        
        // evaluate condition
        yield return Expression;
        // don't jump if true, jump to exit if false
        yield return new ConditionalJumpInstruction(null, exitLabel);
        
        // execute body
        yield return Body;

        // jump target for continue statements
        yield return new LoopContinueLabel();
        // jump back to the condition
        yield return new JumpInstruction(conditionLabel);
        
        // jump target for break statements
        yield return new LoopBreakLabel();
        // exit - we're free from the loop
        yield return new LabelInstruction(exitLabel);
    }
}