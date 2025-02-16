using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Crumpet.Interpreter.Collections;

public class MultiDictionary<TKey, TValue> : IDictionary<TKey, List<TValue>> where TKey : notnull
{
    private readonly Dictionary<TKey, List<TValue>> m_dictionary = new Dictionary<TKey, List<TValue>>();
    
    public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
    {
        return m_dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<TKey, List<TValue>> item)
    {
        if (!m_dictionary.ContainsKey(item.Key))
            m_dictionary[item.Key] = new List<TValue>();
        
        m_dictionary[item.Key].AddRange(item.Value);
    }

    public void Clear()
    {
        m_dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, List<TValue>> item)
    {
        return m_dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, List<TValue>> item)
    {
        return m_dictionary.Remove(item.Key);
    }

    public int Count => m_dictionary.Count;
    
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        if (!m_dictionary.ContainsKey(key))
            m_dictionary[key] = new List<TValue>(1);
        
        m_dictionary[key].Add(value);
    }
    
    public void Add(TKey key, List<TValue> value)
    {
        if (!m_dictionary.ContainsKey(key))
            m_dictionary[key] = new List<TValue>(value.Count);
        
        m_dictionary[key].AddRange(value);
    }

    public bool ContainsKey(TKey key)
    {
        return m_dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return m_dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out List<TValue> value)
    {
        return m_dictionary.TryGetValue(key, out value);
    }

    public List<TValue> this[TKey key]
    {
        get => m_dictionary[key];
        set => m_dictionary[key] = value;
    }

    public ICollection<TKey> Keys => m_dictionary.Keys;
    public ICollection<List<TValue>> Values => m_dictionary.Values;
}