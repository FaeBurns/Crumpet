namespace Crumpet.Tests.Interpreter;

// [TestFixture]
public class FullInterpreterTests
{
    // [TestCase("Interpreter/simpleadd", ExpectedResult = 3)]
    public object RunExampleFile(string filename)
    {
        return RunProgramFile(Path.Combine("Examples//", filename) + ".crm");
    }
    
    public object RunProgramFile(string path)
    {
        return RunProgram(File.ReadAllText(path));
    }
    
    public object RunProgram(string content)
    {
        throw new NotImplementedException();
    }
}