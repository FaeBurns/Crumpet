namespace Crumpet.Exceptions;

public class TypeNotFoundException : Exception
{
    public string Name { get; }

    public TypeNotFoundException(string name, string message) : base(message)
    {
        Name = name;
    }
}