using Crumpet.Language.Nodes;
using Crumpet.Language.Nodes.Expressions;
using Crumpet.Language.Nodes.Statements;
using Crumpet.Language.Nodes.Terminals;
using Parser;

namespace Crumpet.Language;

public class CrumpetNodeFactoryCollection : INodeFactoryCollection
{
    public IEnumerable<Type> GetNonTerminalFactories()
    {
        yield return typeof(RootNonTerminalNode);
        yield return typeof(DeclarationNode);
        yield return typeof(FunctionDeclarationNode);
        yield return typeof(ParameterListNode);
        yield return typeof(ParameterNode);
        yield return typeof(TypeDeclarationNode);
        yield return typeof(TypeDeclarationFieldNode);
        yield return typeof(StatementBodyNode);
        yield return typeof(StatementNode);
        yield return typeof(FlowStatementNode);
        yield return typeof(IfStatementNode);
        yield return typeof(WhileStatementNode);
        yield return typeof(ForStatementNode);
        yield return typeof(TryCatchStatementNode);
        yield return typeof(InitializationStatementNode);
        yield return typeof(TypeNode);
        yield return typeof(ExpressionNode);
        yield return typeof(UnaryExpressionNode);
        yield return typeof(ExpressionWithPostfixNode);
        yield return typeof(ExpressionWithPointerPrefixNode);
        yield return typeof(PrimaryExpressionNode);
        yield return typeof(AssignmentExpressionNode);
        yield return typeof(ArgumentExpressionListNode);
        yield return typeof(LiteralConstantNode);
        yield return typeof(OrExpressionNode);
        yield return typeof(AndExpressionNode);
        yield return typeof(ExclusiveOrExpressionNode);
        yield return typeof(EqualityExpressionNode);
        yield return typeof(RelationExpressionNode);
        yield return typeof(SumExpressionNode);
        yield return typeof(MultExpressionNode);
    }

    public IEnumerable<Type> GetTerminalFactories()
    {
        yield return typeof(BoolLiteralNode);
        yield return typeof(FloatLiteralNode);
        yield return typeof(IdentifierNode);
        yield return typeof(IntLiteralNode);
        yield return typeof(StringLiteralNode);
        yield return typeof(NullLiteralNode);
    }
}