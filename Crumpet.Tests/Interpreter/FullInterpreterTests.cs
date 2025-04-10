using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Shared;

namespace Crumpet.Tests.Interpreter;

[TestFixture]
public class FullInterpreterTests
{
    [TestCase("Interpreter/simpleadd", "2", "1", ExpectedResult = 3)]
    [TestCase("Interpreter/argument_type", ExpectedResult = 0)]
    [TestCase("Interpreter/requirements_test", ExpectedResult = 1)]
    [TestCase("Interpreter/throw_in_catch", ExpectedResult = 0)]
    [TestCase("Interpreter/linked_list", ExpectedResult = 0)]
    [TestCase("Interpreter/ImportTest/import_root", ExpectedResult = 0)]
    public object RunExampleFile(string filename, params string[] args)
    {
        using MemoryStream outputStream = new MemoryStream();
        object result = RunProgramFile(Path.Combine("Examples//", filename) + ".crm", "main", args, null, outputStream);
        outputStream.Seek(0, SeekOrigin.Begin);
        using StreamReader outputReader = new StreamReader(outputStream);
        TestContext.Out.Write(outputReader.ReadToEnd());

        return result;
    }

    [TestCase("Interpreter/map_tests","test_map_create")]
    [TestCase("Interpreter/map_tests","test_map_add")]
    [TestCase("Interpreter/map_tests","test_map_remove")]
    [TestCase("Interpreter/map_tests","test_map_has")]
    [TestCase("Interpreter/generics_tests", "TestConstructDoubleGenericTwoValues")]
    [TestCase("Interpreter/generics_tests", "TestConstructDoubleGenericSingleValue")]
    public void RunTestInFile(string filename, string testName)
    {
        using MemoryStream outputStream = new MemoryStream();
        object result = RunProgramFile(Path.Combine("Examples//", filename) + ".crm", testName, [], null, outputStream);
        outputStream.Seek(0, SeekOrigin.Begin);
        using StreamReader outputReader = new StreamReader(outputStream);
        TestContext.Out.Write(outputReader.ReadToEnd());

        Assert.That(result, Is.EqualTo(1));
    }
    
    public object RunProgramFile(string path, string entryPointName, string[] args, Stream? inputStream = null, Stream? outputStream = null)
    {
        ProgramRuntimeHandler runtimeHandler  = new ProgramRuntimeHandler();
        return runtimeHandler.RunFile(new FileInfo(path), entryPointName, args, inputStream, outputStream);
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
        string[] args = { "output1", "output2" };
        RunProgramFile(path, "main", args, inputStream, outputStream);
        
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
        Assert.Throws<InterpreterException>(() => RunProgramFile(path, "main", []), ExceptionConstants.UNCAUGHT_RUNTIME_EXCEPTION);
    }

    [Test]
    public void TestAssertShowsArgument()
    {
        Assert.Throws<InterpreterException>(() => RunExampleFile("Interpreter/assert_shows_argument"));
    }
}