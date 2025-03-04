using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public class VariableCollection : IVariableCollection
{
    private readonly Dictionary<string, Variable> m_variables = new Dictionary<string, Variable>();

    public InstanceReference Create(VariableInfo info)
    {
        InstanceReference? existing = FindReference(info.Name);

        if (existing is not null)
            throw new InvalidOperationException(ExceptionConstants.VARIABLE_ALREADY_EXISTS.Format(info.Name));
        
        m_variables.Add(info.Name, new Variable(info.Name, info.Type.CreateInstance()));
        return m_variables[info.Name].Instance;
    }

    public InstanceReference? FindReference(string name)
    {
        return m_variables.GetValueOrDefault(name)?.Instance;
    }

    public InstanceReference GetReference(string name)
    {
        if (FindReference(name) is InstanceReference reference)
            return reference;

        throw new ArgumentException(ExceptionConstants.VARIABLE_NOT_FOUND.Format(name));
    }

    public bool Has(string name)
    {
        return m_variables.ContainsKey(name);
    }

    public bool CheckType(string name, TypeInfo type)
    {
        if (FindReference(name) is InstanceReference reference)
            return reference.Type == type;

        return false;
    }
}