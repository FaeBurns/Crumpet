using Crumpet.Language;
using Crumpet.Language.Nodes;
using Crumpet.Language.Nodes.Expressions;
using Crumpet.Language.Nodes.Statements;




using Parser;

namespace Crumpet.Tests.Parser;

[TestFixture]
public class NodeRegistryTests
{
    private ASTNodeRegistry<CrumpetToken> RegisterNodes()
    {
        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();
        return registry;
    }

    [Test]
    public void TestRegisterFactoryCollection()
    {
        RegisterNodes();
    }
    
    [TestCase(CrumpetToken.BOOL)]
    [TestCase(CrumpetToken.FLOAT)]
    [TestCase(CrumpetToken.STRING)]
    [TestCase(CrumpetToken.INT)]
    [TestCase(CrumpetToken.IDENTIFIER)]
    public void TestRegisterTerminal(CrumpetToken token)
    {
        ASTNodeRegistry<CrumpetToken> registry = RegisterNodes();
        Assert.DoesNotThrow(() => registry.GetTerminalDefinition(token));
    }

    [TestCase(typeof(RootNonTerminalNode))]
    [TestCase(typeof(DeclarationNode), 2)]
    [TestCase(typeof(FunctionDeclarationNode))]
    [TestCase(typeof(ParameterListNode))]
    [TestCase(typeof(ParameterNode), 2)]
    [TestCase(typeof(TypeDeclarationNode))]
    [TestCase(typeof(TypeDeclarationFieldNode))]
    [TestCase(typeof(StatementBodyNode))]
    [TestCase(typeof(StatementNode), 6)]
    [TestCase(typeof(FlowStatementNode))]
    [TestCase(typeof(IfStatementNode))]
    [TestCase(typeof(WhileStatementNode))]
    [TestCase(typeof(ForStatementNode))]
    [TestCase(typeof(InitializationStatementNode), 2)]
    [TestCase(typeof(TypeNode), 2)]
    [TestCase(typeof(ExpressionNode))]
    [TestCase(typeof(UnaryExpressionNode))]
    [TestCase(typeof(ExpressionWithPostfixNode), 5)]
    [TestCase(typeof(PrimaryExpressionNode), 3)]
    [TestCase(typeof(AssignmentExpressionNode), 2)]
    [TestCase(typeof(ArgumentExpressionListNode))]
    [TestCase(typeof(LiteralConstantNode), 4)]
    [TestCase(typeof(OrExpressionNode))]
    [TestCase(typeof(AndExpressionNode))]
    [TestCase(typeof(ExclusiveOrExpressionNode))]
    [TestCase(typeof(EqualityExpressionNode), 1)]
    [TestCase(typeof(RelationExpressionNode), 2)]
    [TestCase(typeof(SumExpressionNode), 1)]
    [TestCase(typeof(MultExpressionNode), 1)]
    public void TestRegisterNonTerminal(Type type, int variantCount = 1)
    {
        ASTNodeRegistry<CrumpetToken> registry = RegisterNodes();
        Assert.That(registry.GetNonTerminalDefinitions(type).Count(), Is.EqualTo(variantCount));
    }
}