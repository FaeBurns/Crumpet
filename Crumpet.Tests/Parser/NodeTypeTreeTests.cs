using Crumpet.Language;
using Crumpet.Language.Nodes;


using Parser;

namespace Crumpet.Tests.Parser;

[TestFixture]
public class NodeTypeTreeTests
{
    private NodeTypeTree<CrumpetToken> BuildTree()
    {
        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();
        return new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));
    }
    
    [Test]
    public void TestBuildTree()
    {
        Assert.DoesNotThrow(() => BuildTree());
    }

    [Test]
    public void TestNodeTree()
    {
        NodeTypeTree<CrumpetToken> tree = BuildTree();
        string treeOutput = tree.ToString();
        Console.WriteLine(treeOutput);
    }
}