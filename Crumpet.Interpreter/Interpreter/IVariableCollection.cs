using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter;

public interface IVariableCollection
{
    /// <summary>
    /// Creates a new variable.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public InstanceReference Create(VariableInfo info);

    /// <summary>
    /// Gets a variable. Returns null if nothing was found
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public InstanceReference? FindReference(string name);

    /// <summary>
    /// Gets a variable. Throws if missing. 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public InstanceReference GetReference(string name);
    
    /// <summary>
    /// Checks if a variable exists 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Has(string name);

    /// <summary>
    /// Checks the type of a variable. returns false if not found
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckType(string name, TypeInfo type);
}