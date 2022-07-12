namespace Hoyo.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 转成显示名字
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? ToDescription(this Enum value)
    {
        var type = value.GetType();
        var member = type.GetMember(value.ToString()).FirstOrDefault();
        return member?.ToDescription();
    }
}
