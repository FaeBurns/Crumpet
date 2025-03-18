using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Variables;

public interface IInstanceContainer
{
    public TypeInfo Type { get; }

    public abstract object GetRawValue();
}

public class ValueInstanceContainer<T> : IInstanceContainer where T : struct
{
    private T m_value = default;

    public TypeInfo Type => new BuiltinTypeInfo<T>();
    
    public object GetRawValue()
    {
        return m_value;
    }
}

public class ReferenceInstanceContainer<T> : IInstanceContainer where T : class
{
    private T m_value;
    
    public TypeInfo Type { get; }
    
    public ReferenceInstanceContainer(TypeInfo type, T value)
    {
        m_value = value ?? throw new ArgumentNullException(nameof(value));
        Type = type;
    }
    
    public object GetRawValue()
    {
        return m_value;
    }
}