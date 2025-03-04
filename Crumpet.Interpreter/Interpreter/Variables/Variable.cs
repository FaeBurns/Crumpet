using Crumpet.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Variables;

public class Variable
{
    public Variable(string name, InstanceReference instance)
    {
        Name = name;
        Instance = instance;
    }
    
    public string Name { get; }
    public InstanceReference Instance { get; }
}