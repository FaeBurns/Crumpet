namespace Crumpet.Exceptions;

public class RuntimeException : Exception
{
    public RuntimeException(string message) : base(message)
    {
    }
}

public static class RuntimeExceptionNames
{
    public const string ARGUMENT = "argument";
    public const string TYPE = "type";
}