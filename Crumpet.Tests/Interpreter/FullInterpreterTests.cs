using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Language;
using Crumpet.Language.Nodes;
using Lexer;
using Parser;
using Shared;
using MemoryStream = System.IO.MemoryStream;

namespace Crumpet.Tests.Interpreter;

// [TestFixture]
public class FullInterpreterTests
{
    [TestCase("Interpreter/simpleadd", 2, 1, ExpectedResult = 3)]
    [TestCase("Interpreter/argument_type", ExpectedResult = 0)]
    [TestCase("Interpreter/linked_list", ExpectedResult = 0)]
    [TestCase("Interpreter/requirements_test", ExpectedResult = 0)]
    [TestCase("Interpreter/throw_in_catch", ExpectedResult = 0)]
    public object RunExampleFile(string filename, params object[] args)
    {
        using MemoryStream outputStream = new MemoryStream();
        object result = RunProgramFile(Path.Combine("Examples//", filename) + ".crm", args, null, outputStream);
        outputStream.Seek(0, SeekOrigin.Begin);
        using StreamReader outputReader = new StreamReader(outputStream);
        TestContext.Out.Write(outputReader.ReadToEnd());

        return result;
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
        
        TreeWalkingInterpreter interpreter = new TreeWalkingInterpreter(result.Root!, source, inputStream, outputStream);
        InterpreterExecutor executor;
        try
        {
             executor = interpreter.Run("main", args);
        }
        catch (KeyNotFoundException e)
        {
            if (e.Message.Contains("Function \"main\" not found"))
                Assert.Fail($"Failed to find main function. Last terminal was {result.LastTerminalHit.Terminal} at {result.LastTerminalHit.Token.Location}");
            else throw;
            return null!;
        }
        return executor.StepUntilComplete().GetValue()!;
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
        
        InterpreterDebuggerHelper.RegisterFunction("returnTest1");
        InterpreterDebuggerHelper.RegisterLocation(40, 23);

        string path = Path.Combine("Examples//", "Interpreter/executionzoo") + ".crm";
        object[] args = { new[] { "output1", "output2" } };
        RunProgramFile(path, args, inputStream, outputStream);
        
        outputStream.Seek(0, SeekOrigin.Begin);
        using StreamReader outputReader = new StreamReader(outputStream);
        Assert.That(outputReader.ReadToEnd(), Does.StartWith("output1\noutput2\nwriteback"));
        outputStream.Seek(0, SeekOrigin.Begin);
        TestContext.Out.Write(outputReader.ReadToEnd());
    }

    [Test]
    public void TestBlocksScope()
    {
        string path = Path.Combine("Examples//", "Interpreter/blocks_scope") + ".crm";
        Assert.Throws<InterpreterException>(() => RunProgramFile(path, []), ExceptionConstants.UNCAUGHT_RUNTIME_EXCEPTION);
    }

    [Test]
    public void TestAssertShowsArgument()
    {
        Assert.Throws<InterpreterException>(() => RunExampleFile("Interpreter/assert_shows_argument"));
    }
}