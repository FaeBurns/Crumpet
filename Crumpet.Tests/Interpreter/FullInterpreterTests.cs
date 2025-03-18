using Crumpet.Interpreter;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;

namespace Crumpet.Tests.Interpreter;

// [TestFixture]
public class FullInterpreterTests
{
    [TestCase("Interpreter/simpleadd", 2, 1, ExpectedResult = 3)]
    public object RunExampleFile(string filename, params object[] args)
    {
        return RunProgramFile(Path.Combine("Examples//", filename) + ".crm", args);
    }
    
    public object RunProgramFile(string path, object[] args)
    {
        return RunProgram(File.ReadAllText(path), args);
    }
    
    public object RunProgram(string source, object[] args)
    {
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(source, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE, CrumpetToken.COMMENT);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));
        NodeWalkingParser<CrumpetToken, RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);
        ParseResult<CrumpetToken, RootNonTerminalNode> result = parser.ParseToRoot(tokens);
        Assert.That(result.Success);
        
        TreeWalkingInterpreter interpreter = new TreeWalkingInterpreter(result.Root!);
        InterpreterExecutor executor = interpreter.Run("main", args);
        return executor.StepUntilComplete().Value;
    }
}