namespace Crumpet.Tests.Console;

[TestFixture]
public class ConsoleTests
{
    private async Task Run(string fileName, params string[] args)
    {
        await Crumpet.Console.Program.Main([Path.Combine("Examples//Interpreter//", fileName), ..args]);
    }
    
    [Test]
    public async Task TestMainFunction()
    {
        await Run("requirements_test.crm");
    }

    [Test]
    public async Task TestEntryPoint()
    {
        await Run("generics_tests.crm", "--entry", "TestConstructDoubleGenericTwoValues");
    }

    [Test]
    public async Task TestStringArrayArgsValid()
    {
        await Run("entrypoint_tests.crm", "--entry", "test_string_array", "string1", "string1");
    }
    
    [Test]
    public async Task TestStringArrayArgsNoArgs()
    {
        await Run("entrypoint_tests.crm", "--entry", "test_string_array");
    }

    [Test]
    public async Task TestConsoleOutputTests()
    {
        await Run("entrypoint_tests.crm", "--entry", "test_string_array");
    }
}