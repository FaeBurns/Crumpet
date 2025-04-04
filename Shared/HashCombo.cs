namespace Shared;

public struct HashCombo
{
    private int m_hashCode;

    public HashCombo()
    {
        m_hashCode = 0;
    }

    public HashCombo(int value)
    {
        m_hashCode = value;
    }
    
    public HashCombo(object initial)
    {
        m_hashCode = initial.GetHashCode();
    }

    public HashCombo(ValueType valueType)
    {
        m_hashCode = valueType.GetHashCode();
    }

    public HashCombo Add<T>(T value)
    {
        m_hashCode = HashCode.Combine(m_hashCode, value);
        return this;
    }

    public override int GetHashCode()
    {
        return m_hashCode;
    }
}