using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Shared;

[SuppressMessage("ReSharper", "StaticMemberInGenericType")]
public readonly struct ID<T>
{
    private static readonly Dictionary<string, int> s_idMap = new Dictionary<string, int>();
    private static readonly Dictionary<int, string> s_nameMap = new Dictionary<int, string>();
    private static int s_nextId;
    private readonly int m_id;

    public ID(string id)
    {
        if (s_idMap.TryGetValue(id, out int value))
        {
            m_id = value;
        }
        else
        {
            m_id = s_nextId++;
            s_idMap.Add(id, m_id);
            s_nameMap.Add(m_id, id);
        }
    }

    private bool Equals(ID<T> other)
    {
        return m_id == other.m_id;
    }

    public override bool Equals(object? obj)
    {
        return obj is ID<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return m_id;
    }

    public static bool operator == (ID<T>? left, ID<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator != (ID<T>? left, ID<T>? right)
    {
        return !Equals(left, right);
    }
    
    public override string ToString()
    {
        return s_nameMap[m_id];
    }
}