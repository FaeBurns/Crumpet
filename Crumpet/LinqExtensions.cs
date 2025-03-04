namespace Crumpet;

public static class LinqExtensions
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            action.Invoke(element);
        }
    }

    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            set.Add(item);
        }
    }
}