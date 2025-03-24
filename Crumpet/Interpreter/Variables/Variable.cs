using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Variables;

public class Variable
{
    private object? m_value;
    
    public TypeInfo Type { get; }
    public VariableModifier Modifier { get; }
    
    public string SourceName { get; set; } = String.Empty;

    private Variable(TypeInfo type, object? value, VariableModifier modifier)
    {
        Type = type;
        m_value = value;
        Modifier = modifier;
    }

    public static Variable Create(TypeInfo type) => type.CreateVariable();
    public static Variable Create(TypeInfo type, object? value) => new Variable(type, value, VariableModifier.COPY);

    public static Variable CreateCopy(Variable existing)
    {
        switch (existing.Modifier)
        {
            case VariableModifier.COPY:
                return new Variable(existing.Type, existing.Type.CreateCopy(existing.GetValue()), VariableModifier.COPY);
                break;
            case VariableModifier.POINTER:
                return new Variable(existing.Type, existing.GetValue(), VariableModifier.POINTER);
                break;
            default:
                throw new UnreachableException();
        }
    }

    public static Variable CreatePointer(TypeInfo type, Variable? target)
    {
        return new Variable(type, target ?? NullTypeInfo.Create(), VariableModifier.POINTER);
    }
    
    public static Variable CreateModifier(TypeInfo defType, VariableModifier modifier, Variable? valueSource)
    {
        switch (modifier)
        {
            case VariableModifier.COPY:
                return CreateCopy(valueSource ?? defType.CreateVariable());
            case VariableModifier.POINTER:
                return CreatePointer(defType, valueSource);
            default:
                throw new UnreachableException();
        }
    }
    
    public object? GetValue() => m_value;
    public T GetValue<T>()
    {
        if (m_value is not T)
        {
            Debugger.Break();
        }
        return (T)(GetValue() ?? throw new NullReferenceException());
    }

    public void SetValue(object? value)
    {
        if (value is Variable variable)
        {
            SetValue(variable);
            return;
        }

        if (Modifier == VariableModifier.POINTER)
            throw new InvalidOperationException(ExceptionConstants.MODIFIER_MISMATCH.Format(VariableModifier.POINTER, Modifier));
        
        m_value = value;
    }

    private void SetValue(Variable source)
    {
        switch (Modifier)
        {
            case VariableModifier.COPY:
                SetValueCopy(source);
                break;
            case VariableModifier.POINTER:
                SetValuePointer(source);
                break;
            default:
                throw new UnreachableException();
        }
    }

    private void SetValueCopy(Variable source)
    {
        if (!source.Type.IsAssignableTo(Type))
            throw new TypeMismatchException(Type, source.Type);
        
        switch (source.Modifier)
        {
            case VariableModifier.COPY:
                m_value = source.Type.ConvertValidObjectTo(Type, source.GetValue());
                break;
            case VariableModifier.POINTER:
                // cannot assign a copy to a pointer
                throw new InvalidOperationException(ExceptionConstants.MODIFIER_MISMATCH.Format(VariableModifier.COPY, VariableModifier.POINTER));
            default:
                throw new UnreachableException();
        }
    }

    private void SetValuePointer(Variable source)
    {
        // types must be equal
        if (Type != source.Type && source.Type is not NullTypeInfo)
            throw new TypeMismatchException(Type, source.Type);
        
        switch (source.Modifier)
        {
            case VariableModifier.POINTER:
                m_value = source.Dereference(); // point to pointed value
                break;
            case VariableModifier.COPY:
                m_value = source; // assign to variable directly
                break;
            default:
                throw new UnreachableException();
        }
    }

    public Variable Dereference()
    {
        if (Modifier != VariableModifier.POINTER)
            throw new InvalidOperationException(ExceptionConstants.MODIFIER_MISMATCH.Format(VariableModifier.POINTER, Modifier));

        if (m_value is null)
            return NullTypeInfo.Create();
        
        Debug.Assert(m_value is Variable);
        
        return (Variable)m_value;
    }
    
    public T DereferenceToValue<T>() => Dereference().GetValue<T>();
    public object? DereferenceToValue() => Dereference().GetValue();

    public bool AssertType<T>()
    {
        return Type == new BuiltinTypeInfo<T>();
    }

    public object? DereferenceOrGetValue()
    {
        if (m_value is Variable variable)
            return variable.DereferenceOrGetValue();

        return m_value;
    }

    public Variable DereferenceToLowestVariable()
    {
        if (Modifier == VariableModifier.COPY)
            return this;
        
        if (Type is NullTypeInfo)
            throw new NullReferenceException();
        
        if (m_value is Variable variable)
        {
            return variable.DereferenceToLowestVariable();
        }

        throw new NullReferenceException();
    }

    protected bool Equals(Variable other)
    {
        if (Type != other.Type)
            return false;

        if (Modifier != other.Modifier)
            return false;

        return m_value == other.m_value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Variable)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, (int)Modifier);
    }

    public static bool operator == (Variable left, Variable right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Variable left, Variable right)
    {
        return !(left == right);
    }
}