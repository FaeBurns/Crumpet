namespace Crumpet.Exceptions;

public class GenericsException : Exception
{
    public GenericsException(string message) : base(message) { }
    
    public GenericsException(string message, Exception innerException) : base(message, innerException) { }
    
    public GenericsException(Exception innerException) : base("", innerException) { }
}