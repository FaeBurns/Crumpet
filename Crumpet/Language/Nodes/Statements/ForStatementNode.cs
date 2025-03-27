using Crumpet.Instructions;
using Crumpet.Instructions.Details;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class ForStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public NonTerminalNode Initializer { get; }
    public ExpressionNode Expression { get; }
    public NonTerminalNode Finalizer { get; }
    public StatementBodyNode Body { get; }

    public ForStatementNode(NonTerminalNode initializer, ExpressionNode expression, NonTerminalNode finalizer, StatementBodyNode body) : base(initializer, expression, finalizer, body)
    {
        Initializer = initializer;
        Expression = expression;
        Finalizer = finalizer;
        Body = body;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // using StatementNodes is not as restrictive as I'd like
        // e.g. it's technically possible for the following
        // for(for(for( ...
        // but it's a whole lot more nodes to add in order to fix that
        yield return new NonTerminalDefinition<ForStatementNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_FOR),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OrConstraint(
                    new NonTerminalConstraint<InitializationStatementNode>(),
                    new NonTerminalConstraint<AssignmentExpressionNode>()), // int i = 0;
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON),
                new NonTerminalConstraint<ExpressionNode>(), // i < 10;
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON),
                new NonTerminalConstraint<ExpressionWithPostfixNode>(), // i++
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<ForStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        // hold everything inside it's own executable unit - helps make label searching easier during execution
        yield return new ExecuteUnitInstruction(new InstructionCollator(GetLoopInstructions()), Location);
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
        
        // have the initializer above the loop label so it doesn't get called again
        yield return Initializer;
        
        // start of loop label
        yield return new LabelInstruction(conditionLabel, Location){FriendlyName = "For Condition"};
        
        // evaluate condition
        yield return Expression;
        // don't jump if true, jump to exit if false
        yield return new ConditionalJumpInstruction(null, exitLabel, Location);
        
        // execute body
        yield return Body;

        // jump target for continue statements
        yield return new LoopContinueLabel(Location);
        
        // the i++ will push its result to the stack. Ensure this does not cause an issue
        StackItemCounter counter = new StackItemCounter();
        yield return new SaveStackItemCountInstruction(counter, Location);
        // loop finalizer (i++)
        yield return Finalizer;
        yield return new RestoreStackItemCountInstruction(counter, Location);
        
        // jump back to the condition
        yield return new JumpInstruction(conditionLabel, Location);
        
        // jump target for break statements
        yield return new LoopBreakLabel(Location);
        // exit - we're free from the loop
        yield return new LabelInstruction(exitLabel, Location) {FriendlyName = "For Exit"};
    }
}