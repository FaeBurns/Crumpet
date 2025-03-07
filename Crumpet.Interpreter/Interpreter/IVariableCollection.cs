using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter;

public interface IVariableCollection
{
    /// <summary>
    /// Creates a new variable.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public Variable Create(VariableInfo info);

    /// <summary>
    /// Adds an existing variable.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="variable"></param>
    /// <returns>True if the variable was added, False if one already existed.</returns>
    public bool Add(string name, Variable variable);

    /// <summary>
    /// Gets a variable. Returns null if nothing was found
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Variable? FindVariable(string name);

    /// <summary>
    /// Gets a variable. Throws if missing.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Variable GetVariable(string name);

    /// <summary>
    /// Checks if a variable exists
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Has(string name);

    /// <summary>
    /// Checks the type of a variable. returns false if not found or if the type mismatched.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckType(string name, TypeInfo type);
}