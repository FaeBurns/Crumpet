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
}