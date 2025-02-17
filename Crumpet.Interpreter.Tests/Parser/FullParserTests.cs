using Crumpet.Interpreter.Lexer;
using Crumpet.Interpreter.Parser;
using Crumpet.Language;
using Crumpet.Language.Nodes;

namespace Crumpet.Interpreter.Tests.Parser;

[TestFixture]
public class FullParserTests
{
    [TestCase("Full/zoo")]
    public void TestExampleFile(string examplePath)
    {
        string source = File.ReadAllText(Path.Combine("Examples//", examplePath) + ".crm");
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(source, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));

        NodeWalkingParser<CrumpetToken,RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);

        RootNonTerminalNode? rootNode = parser.ParseToRoot(tokens);
        
        Assert.That(rootNode, Is.Not.Null);
    }
}