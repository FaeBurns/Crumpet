using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter;

public class Scope : IVariableCollection
{
    private readonly VariableCollection m_variables = new VariableCollection();
    public Scope? Parent { get; }

    public Scope(Scope? parent)
    {
        Parent = parent;
    }

    public Variable Create(VariableInfo info) => m_variables.Create(info);

    public bool Add(string name, Variable variable) => m_variables.Add(name, variable);

    public Variable? FindVariable(string name)
    {
        if (m_variables.FindVariable(name) is Variable variable)
            return variable;

        if (Parent is not null)
            return Parent?.FindVariable(name);

        return null;
    }

    public Variable GetVariable(string name)
    {
        if (FindVariable(name) is Variable variable)
            return variable;

        throw new KeyNotFoundException(ExceptionConstants.VARIABLE_NOT_FOUND.Format(name));
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
        if (FindVariable(name) is Variable variable) return variable.Type == type;

        return false;
    }
}