﻿using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

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

        m_variables.Add(info.Name, Variable.CreateModifier(info.Type, info.VariableModifier, null!));
        return m_variables[info.Name];
    }

    public bool Add(string name, Variable variable)
    {
        return m_variables.TryAdd(name, variable);
    }

    public Variable Create(string name, TypeInfo type, VariableModifier modifier)
    {
        return Create(new VariableInfo(name, type, modifier));
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

    protected bool Equals(VariableCollection other)
    {
        return m_variables.Equals(other.m_variables);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((VariableCollection)obj);
    }

    public override int GetHashCode()
    {
        int hashcode = 1430287;
        foreach (Variable var in m_variables.Values)
        {
            hashcode = HashCode.Combine(hashcode, var.GetObjectHashCode());
        }

        return hashcode;
    }
}