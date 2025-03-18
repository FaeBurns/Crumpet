using System.Reflection;

namespace Shared;

public static class Extensions
{
    public static string Format(this string str, params object[] args)
    {
        return String.Format(str, args);
    }
    
    public static TAttribute GetEnumAttribute<TAttribute>(this Enum enumVal) where TAttribute : Attribute
    {
        Type type = enumVal.GetType();
        MemberInfo[] memberInfos = type.GetMember(Enum.GetName(enumVal.GetType(), enumVal)!);
        return memberInfos[0].GetCustomAttribute<TAttribute>()!;
    }

    public static int IndexOf<T>(this IReadOnlyList<T> list, T target)
    {
        int i = 0;
        foreach (T element in list)
        {
            if (Equals(element, target))
                return i;
            i++;
        }
        return -1;
    }
}