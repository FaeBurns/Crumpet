using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class StatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public ASTNode? VariantNode { get; }

    protected StatementNode(ASTNode? variantNode) : base(variantNode!)
    {
        VariantNode = variantNode;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<StatementNode>(
            new SequenceConstraint(
                new OptionalConstraint(new NonTerminalConstraint<ExpressionNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<StatementNode>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<IfStatementNode>(),
            GetNodeConstructor<StatementNode>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<WhileStatementNode>(),
            GetNodeConstructor<StatementNode>());
        
        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<ForStatementNode>(),
            GetNodeConstructor<StatementNode>());

        yield return new NonTerminalDefinition<StatementNode>(
            new NonTerminalConstraint<FlowStatementNode>(),
            GetNodeConstructor<StatementNode>());

        yield return new NonTerminalDefinition<StatementNode>(
            new SequenceConstraint(
                new OptionalConstraint(new NonTerminalConstraint<InitializationStatementNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<StatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return VariantNode;
    }
}