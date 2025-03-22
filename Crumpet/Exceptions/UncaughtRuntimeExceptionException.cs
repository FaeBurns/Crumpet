namespace Crumpet.Exceptions;

public class UncaughtRuntimeExceptionException : Exception
{
    public UncaughtRuntimeExceptionException(string message) : base(message)
    {
    }
}