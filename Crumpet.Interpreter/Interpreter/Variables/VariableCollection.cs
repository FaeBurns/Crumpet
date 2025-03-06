using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class VariableCollection : IVariableCollection
{
    private readonly Dictionary<string, Variable> m_variables = new Dictionary<string, Variable>();

    public Variable this[string key] => m_variables[key];
    
    public IEnumerable<string> VariableNames => m_variables.Keys;
    
    public Variable Create(VariableInfo info)
    {
        Variable? existing = FindVariable(info.Name);

        if (existing is not null)
            throw new InvalidOperationException(ExceptionConstants.VARIABLE_ALREADY_EXISTS.Format(info.Name));
        
        m_variables.Add(info.Name, new Variable(info.Name, info.Type.CreateInstance(), info.VariableModifier));
        return m_variables[info.Name];
    }

    public Variable? FindVariable(string name)
    {
        return m_variables.GetValueOrDefault(name);
    }

    public Variable GetVariable(string name)
    {
        if (FindVariable(name) is Variable variable)
            return variable;

        throw new ArgumentException(ExceptionConstants.VARIABLE_NOT_FOUND.Format(name));
    }

    public bool Has(string name)
    {
        return m_variables.ContainsKey(name);
    }

    public bool CheckType(string name, TypeInfo type)
    {
        if (FindVariable(name) is Variable variable)
            return variable.Type == type;

        return false;
    }
}