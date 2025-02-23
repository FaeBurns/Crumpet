using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Interpreter.Variables;

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