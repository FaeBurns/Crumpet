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
}