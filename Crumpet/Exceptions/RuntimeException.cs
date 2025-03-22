namespace Crumpet.Exceptions;

public class RuntimeException : Exception
{
    public string Name { get; }

    public RuntimeException(string name)
    {
        Name = name;
    }

    public RuntimeException(string name, string message) : base(message)
    {
        Name = name;
    }
}

public static class RuntimeExceptionNames
{
    public const string ARGUMENT = "argument";
    public const string TYPE = "type";
    public const string ASSERT = "assert";
    public const string UNCAUGHT = "uncaught";
}