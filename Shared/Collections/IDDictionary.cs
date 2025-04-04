using System.Runtime.Serialization;

namespace Shared.Collections;

public class IDDictionary<T> : Dictionary<ID<T>, T>
{
    public IDDictionary()
    {
    }

    public IDDictionary(IDictionary<ID<T>, T> dictionary) : base(dictionary)
    {
    }

    public IDDictionary(IDictionary<ID<T>, T> dictionary, IEqualityComparer<ID<T>>? comparer) : base(dictionary, comparer)
    {
    }

    public IDDictionary(IEnumerable<KeyValuePair<ID<T>, T>> collection) : base(collection)
    {
    }

    public IDDictionary(IEnumerable<KeyValuePair<ID<T>, T>> collection, IEqualityComparer<ID<T>>? comparer) : base(collection, comparer)
    {
    }

    public IDDictionary(IEqualityComparer<ID<T>>? comparer) : base(comparer)
    {
    }

    public IDDictionary(int capacity) : base(capacity)
    {
    }

    public IDDictionary(int capacity, IEqualityComparer<ID<T>>? comparer) : base(capacity, comparer)
    {
    }
}