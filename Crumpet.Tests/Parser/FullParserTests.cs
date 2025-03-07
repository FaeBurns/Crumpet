using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;

namespace Crumpet.Tests.Parser;

[TestFixture]
public class FullParserTests
{
    [TestCase("Parser/fullzoo")]
    public void TestExampleFile(string path)
    {
        ParseExampleFile(path);
    }

    public ParseResult<CrumpetToken, RootNonTerminalNode> ParseExampleFile(string examplePath)
    {
        string source = File.ReadAllText(Path.Combine("Examples//", examplePath) + ".crm");
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(source, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE, CrumpetToken.COMMENT);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));

        NodeWalkingParser<CrumpetToken,RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);

        ParseResult<CrumpetToken, RootNonTerminalNode> result = parser.ParseToRoot(tokens);

        TestContext.WriteLine($"Last token: {result.LastTerminalHit} at {result.LastTerminalHit.Location}");

        Assert.That(result.Root, Is.Not.Null);

        return result;
    }

    [Test]
    public void TestZooFile()
    {
        ParseResult<CrumpetToken,RootNonTerminalNode> result = ParseExampleFile("Parser/zoo");
        Assert.That(result.Root!.Declarations, Has.Length.EqualTo(3));
        Assert.That(result.Root!.Declarations.Select(d => d.Variant.GetType()), Is.EquivalentTo(new [] {typeof(TypeDeclarationNode), typeof(FunctionDeclarationNode), typeof(FunctionDeclarationNode)}));
    }
}