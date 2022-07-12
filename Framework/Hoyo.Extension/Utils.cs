using Hoyo.Extensions;

namespace Hoyo.Extension;
public static class Utils
{
    /// <summary>
    /// 检查参数不能为空引用，否则抛出<see cref="ArgumentNullException"/>异常。
    /// </summary>
    /// <param name="value">要验证的值</param>
    /// <param name="paramName">参数名称</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static T NotNull<T>(T value, string paramName)
    {
        ObjectExtension.Require<ArgumentNullException>(value is not null, paramName);
        return value;
    }

    /// <summary>
    /// 检查字符串不能为空引用或空字符串，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName">参数名称。</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string NullOrWhiteSpace(string value, string paramName)
    {
        ObjectExtension.Require<ArgumentException>(!string.IsNullOrWhiteSpace(value), paramName);
        return value;
    }
}
