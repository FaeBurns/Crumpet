using Crumpet.Language;
using Parser;

namespace Crumpet.Tests;

[SetUpFixture]
public class TestsSetup
{
    [OneTimeSetUp]
    public void LoadInstructions()
    {
        ParserDebuggerHelper<CrumpetToken>.Clear();
    }
}