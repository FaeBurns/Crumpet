using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Variables;

public class Variable
{
    // will be set in constructor via Value's setter
    private object m_value = null!;

    // used with reference or pointer modifiers
    private Variable? m_referencedVariable;

    public VariableModifier Modifier { get; }

    public TypeInfo Type { get; }

    public object Value
    {
        get => GetValue();
        set => SetValue(value);
    }

    public T GetValue<T>()
    {
        if (!Value.GetType().IsAssignableTo(typeof(T)))
            throw new TypeMismatchException(typeof(T), Value.GetType());

        return (T)Value;
    }

    private Variable(TypeInfo type, object initialValue, VariableModifier modifier)
    {
        Type = type;
        Modifier = modifier;

        // only assign value on copy, other modifiers will handle it themselves
        if (modifier == VariableModifier.COPY)
            Value = initialValue;
    }

    public static Variable Create(TypeInfo type) => type.CreateVariable();

    public static Variable Create(TypeInfo type, object initialValue)
    {
        return new Variable(type, initialValue ?? throw new ArgumentNullException(nameof(initialValue)), VariableModifier.COPY);
    }

    public static Variable CreateCopy(Variable source)
    {
        return new Variable(source.Type, source.Value, VariableModifier.COPY);
    }

    public static Variable CreatePointer(Variable target)
    {
        return new Variable(target.Type, null!, VariableModifier.POINTER)
        {
            m_referencedVariable = target,
        };
    }

    public static Variable CreateModifier(TypeInfo type, VariableModifier modifier, Variable potentiallyReferencedVariable)
    {
        return modifier switch
        {
            VariableModifier.COPY => Create(type),
            VariableModifier.POINTER => CreatePointer(potentiallyReferencedVariable),
            _ => throw new UnreachableException(),
        };
    }

    public bool AssertType(TypeInfo type)
    {
        return Type == type;
    }

    public bool AssertType<T>() where T : struct
    {
        return AssertType(new BuiltinTypeInfo<T>());
    }

    private object GetValue()
    {
        // assume m_referencedVariable is never null with relevant modifiers
        return Modifier switch
        {
            VariableModifier.COPY => m_value,
            VariableModifier.POINTER => m_referencedVariable!.m_value,
            _ => throw new UnreachableException(),
        };
    }

    private void SetValue(object value)
    {
        switch (Modifier)
        {
            case VariableModifier.COPY:
                if (value is Variable varCopy)
                    m_value = Type.CreateCopy(varCopy.Value);
                else
                    m_value = Type.CreateCopy(value);
                break;
            case VariableModifier.POINTER:
                // set value on target pointed variable
                m_referencedVariable!.Value = value;
                break;
            default:
                throw new UnreachableException();
        }
    }
}