using System.Diagnostics;
using Crumpet.Interpreter.Lexer;
using Crumpet.Interpreter.Parser;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Crumpet.Language.Nodes.Expressions;
using Crumpet.Language.Nodes.Statements;

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

        // ParserDebuggerHelper<CrumpetToken>.SetBreakingTerminalContent("bar");
        // ParserDebuggerHelper<CrumpetToken>.SetBreakingNonTerminals(typeof(ExpressionWithPostfixNodeIdentifierVariant));
        
        ParseResult<CrumpetToken, RootNonTerminalNode> result = parser.ParseToRoot(tokens);
        
        TestContext.WriteLine($"Last token: {result.LastTerminalHit} at {result.LastTerminalHit.Location}");
        
        Assert.That(result.Root, Is.Not.Null);
    }
}