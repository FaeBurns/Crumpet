using System.Reflection;

namespace Crumpet.Interpreter;

public static class Extensions
{
    public static string Format(this string str, params object[] args)
    {
        return String.Format(str, args);
    }

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
    
    public static TAttribute GetEnumAttribute<TAttribute>(this Enum enumVal) where TAttribute : Attribute
    {
        Type type = enumVal.GetType();
        MemberInfo[] memberInfos = type.GetMember(Enum.GetName(enumVal.GetType(), enumVal)!);
        return memberInfos[0].GetCustomAttribute<TAttribute>()!;
    }
}