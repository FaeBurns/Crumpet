using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Interpreter.Variables;
using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Interpreter;

public class Scope : IVariableCollection
{
    private readonly VariableCollection m_variables = new VariableCollection();
    public Scope? Parent { get; }

    public Scope(Scope? parent)
    {
        Parent = parent;
    }

    
    public InstanceReference Create(VariableInfo info) => m_variables.Create(info);

    public InstanceReference? FindReference(string name)
    {
        if (m_variables.FindReference(name) is InstanceReference reference)
            return reference;

        if (Parent is not null)
            return Parent?.FindReference(name);

        return null;
    }

    public InstanceReference GetReference(string name)
    {
        if (FindReference(name) is InstanceReference reference)
            return reference;

        throw new ArgumentException(ExceptionConstants.VARIABLE_NOT_FOUND.Format(name));
    }

    public bool Has(string name)
    {
        if (m_variables.Has(name)) return true;
        
        if (Parent is not null)
            return Parent.Has(name);

        return false;
    }

    public bool CheckType(string name, TypeInfo type)
    {
        if (FindReference(name) is InstanceReference reference) return reference.Type == type;
        
        return false;
    }
}