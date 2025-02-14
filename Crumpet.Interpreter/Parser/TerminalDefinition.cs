using System.Reflection;

namespace Crumpet.Interpreter.Parser;

public sealed class TerminalDefinition
{
    public string Name { get; }
    public string Regex { get; }
    public ConstructorInfo Constructor { get; }

    public TerminalDefinition(string name, string regex, ConstructorInfo constructor)
    {
        Name = name;
        Regex = regex;
        Constructor = constructor;
    }
}