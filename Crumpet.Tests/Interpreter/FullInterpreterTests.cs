using Crumpet.Interpreter;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;
using MemoryStream = System.IO.MemoryStream;

namespace Crumpet.Tests.Interpreter;

// [TestFixture]
public class FullInterpreterTests
{
    [TestCase("Interpreter/simpleadd", 2, 1, ExpectedResult = 3)]
    public object RunExampleFile(string filename, params object[] args)
    {
        return RunProgramFile(Path.Combine("Examples//", filename) + ".crm", args);
    }
    
    public object RunProgramFile(string path, object[] args, Stream? inputStream = null, Stream? outputStream = null)
    {
        return RunProgram(File.ReadAllText(path), args, inputStream, outputStream);
    }
    
    public object RunProgram(string source, object[] args, Stream? inputStream = null, Stream? outputStream = null)
    {
        ILexer<CrumpetToken> lexer = new Lexer<CrumpetToken>(source, CrumpetToken.WHITESPACE, CrumpetToken.NEWLINE, CrumpetToken.COMMENT);
        IEnumerable<Token<CrumpetToken>> tokens = lexer.Tokenize();

        ASTNodeRegistry<CrumpetToken> registry = new ASTNodeRegistry<CrumpetToken>();
        registry.RegisterFactoryCollection<CrumpetNodeFactoryCollection>();

        NodeTypeTree<CrumpetToken> nodeTree = new NodeTypeTree<CrumpetToken>(registry, typeof(RootNonTerminalNode));
        NodeWalkingParser<CrumpetToken, RootNonTerminalNode> parser = new NodeWalkingParser<CrumpetToken, RootNonTerminalNode>(registry, nodeTree);
        ParseResult<CrumpetToken, RootNonTerminalNode> result = parser.ParseToRoot(tokens);
        Assert.That(result.Success);
        
        TreeWalkingInterpreter interpreter = new TreeWalkingInterpreter(result.Root!, inputStream, outputStream);
        InterpreterExecutor executor = interpreter.Run("main", args);
        return executor.StepUntilComplete().Value;
    }

    [Test]
    public void TestExecutionZoo()
    {
        using MemoryStream inputStream = new MemoryStream();
        using StreamWriter inputStreamWriter = new StreamWriter(inputStream);
        inputStreamWriter.WriteLine("0");
        inputStreamWriter.WriteLine("writeback");
        inputStreamWriter.Flush();
        inputStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream outputStream = new MemoryStream();

        string path = Path.Combine("Examples//", "Interpreter/executionzoo") + ".crm";
        object[] args = { new[] { "output1", "output2" } };
        RunProgramFile(path, args, inputStream, outputStream);
        
        outputStream.Seek(0, SeekOrigin.Begin);
        using StreamReader outputReader = new StreamReader(outputStream);
        Assert.That(outputReader.ReadToEnd(), Does.StartWith("output1\noutput2\nwriteback"));
        outputStream.Seek(0, SeekOrigin.Begin);
        Assert.That(outputReader.ReadToEnd(), Does.EndWith("output1\n"));
    }
}