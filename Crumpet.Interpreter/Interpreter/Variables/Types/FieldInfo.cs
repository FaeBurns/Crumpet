namespace Crumpet.Interpreter.Variables.Types;

public class FieldInfo
{
    public FieldInfo(string name, TypeInfo type, bool isReference = false)
    {
        Name = name;
        Type = type;
        IsReference = isReference;
    }

    public string Name { get; }
    public TypeInfo Type { get; set; }
    public bool IsReference { get; }
}