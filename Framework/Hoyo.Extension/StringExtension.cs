﻿using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Hoyo.Extensions;
/// <summary>
/// 字符串String扩展
/// </summary>
public static class StringExtension
{
    #region 重复字符串
    /// <summary>
    /// 将指定字符串，按照串联的方式重复一定次数
    /// </summary>
    /// <param name="value">要重复的字符串</param>
    /// <param name="count">重复的次数</param>
    /// <returns>字符串value重复count次后的串联字符串</returns>
    public static string ReplicateString(this string value, int count) => string.Join(value, new string[count + 1]);
    /// <summary>
    /// 将指定字符，按照串联的方式重复一定次数
    /// </summary>
    /// <param name="c">要重复的字符</param>
    /// <param name="count">重复的次数</param>
    /// <returns>字符c重复count次后的串联字符串</returns>
    public static string ReplicateString(this char c, int count) => new(c, count);
    #endregion

    #region 字符串转为日期
    /// <summary>
    /// 将格式化日期串转化为相应的日期
    /// （比如2004/05/06，2004-05-06 12:00:03，12:23:33.333等）
    /// </summary>
    /// <param name="value">日期格式化串</param>		
    /// <returns>转换后的日期，对于不能转化的返回DateTime.MinValue</returns>
    public static DateTime ToDateTime(this string value) => ToDateTime(value, DateTime.MinValue);
    /// <summary>
    /// 将格式化日期串转化为相应的日期
    /// （比如2004/05/06，2004-05-06 12:00:03，12:23:33.333等）
    /// </summary>
    /// <param name="value">日期格式化串</param>
    /// <param name="defaultValue">当为空或错误时的返回日期</param>
    /// <returns>转换后的日期</returns>
    public static DateTime ToDateTime(this string value, DateTime defaultValue)
    {
        var result = DateTime.MinValue;
        return string.IsNullOrEmpty(value) || DateTime.TryParse(value, out result) ? result : defaultValue;
    }
    /// <summary>
    /// 从字符串获取DateTime?,支持的字符串格式:'2020/10/01,2020-10-01,20201001,2020.10.01'
    /// </summary>
    /// <param name="value"></param>
    /// <param name="force">true:当无法转换成功时抛出异常.false:当无法转化成功时返回null</param>
    public static DateTime? ToDateTime(this string value, bool force)
    {
        value = value.Replace("/", "-").Replace(".", "-").Replace("。", "-").Replace(",", "-").Replace(" ", "-").Replace("|", "-");
        if (value.Split('-').Length == 1 && value.Length == 8) value = string.Join("-", value[..4], value.Substring(4, 2), value.Substring(6, 2));
        return DateTime.TryParse(value, out var date) switch
        {
            false => force ? throw new("string format is not correct,must like:2020/10/01,2020-10-01,20201001,2020.10.01") : null,
            _ => date,
        };
    }
    /// <summary>
    /// 将字符串转化为固定日期格式字符串,如:20180506 --> 2018-05-06
    /// </summary>
    /// <exception cref="FormatException"></exception>
    public static string ToDateTimeFormat(this string value, bool force = true)
    {
        value = value.Replace("/", "-").Replace(".", "-").Replace("。", "-").Replace(",", "-").Replace(" ", "-").Replace("|", "-");
        if (value.Split('-').Length == 1 && value.Length == 8) value = string.Join("-", value[..4], value.Substring(4, 2), value.Substring(6, 2));
        return DateTime.TryParse(value, out _)
            ? value
            : force ? throw new("string format is not correct,must like:2020/10/01,2020-10-01,20201001,2020.10.01") : string.Empty;
    }
#if !NETSTANDARD
    /// <summary>
    /// 获取某个日期串的DateOnly
    /// </summary>
    /// <param name="value">格式如: 2022-02-28</param>
    /// <returns></returns>
    public static DateOnly ToDateOnly(this string value) => DateOnly.FromDateTime(value.ToDateTime());
    /// <summary>
    /// 获取某个时间串的TimeOnly
    /// </summary>
    /// <param name="value">格式如: 23:20:10</param>
    /// <returns></returns>
    public static TimeOnly ToTimeOnly(this string value) => TimeOnly.FromDateTime($"{DateTime.Now:yyyy-MM-dd} {value}".ToDateTime());
#endif
    #endregion

    #region 以特定字符串间隔的字符串转化为字符串集合
    /// <summary>
    /// 以特定字符间隔的字符串转化为字符串集合
    /// </summary>
    /// <param name="value">需要处理的字符串</param>
    /// <param name="separator">分隔此实例中子字符串</param>
    /// <returns>转化后的字符串集合，如果传入数组为null则返回空集合</returns>
    public static StringCollection ToStringCollection(this string value, string separator)
    {
        var col = new StringCollection();
        if (string.IsNullOrEmpty(separator) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
        {
            return col;
        }
        var index = 0;
        var pos = 0;
        var len = separator.Length;
        while (pos >= 0)
        {
            pos = value.IndexOf(separator, index, StringComparison.CurrentCultureIgnoreCase);
            _ = pos >= 0 ? col.Add(value[index..pos]) : col.Add(value[index..]);
            index = pos + len;
        }
        return col;
    }
    #endregion

    #region 将字符串中的单词首字母大写或者小写
    /// <summary>
    /// 将字符串中的单词首字母大写或者小写
    /// </summary>
    /// <param name="value">单词</param>
    /// <param name="lower">是否小写? 默认:true</param>
    /// <returns></returns>
    public static string ToTitleUpperCase(this string value, bool lower = true)
    {
        var regex = new Regex(@"\w+");
        return regex.Replace(value,
            delegate (Match m)
            {
                var str = m.ToString();
                if (!char.IsLower(str[0])) return str;
                var header = lower ? char.ToLower(str[0], CultureInfo.CurrentCulture) : char.ToUpper(str[0], CultureInfo.CurrentCulture);
                return $"{header}{str[1..]}";
            });
    }
    #endregion

    #region 将字符串转为整数,数组,内存流,GUID(GUID需要字符串本身为GUID格式)
    /// <summary>
    /// 将字符串转化为内存字节流
    /// </summary>
    /// <param name="value">需转换的字符串</param>
    /// <param name="encoding">编码类型</param>
    /// <returns>字节流</returns>
    public static MemoryStream ToStream(this string value, Encoding encoding)
    {
        using var mStream = new MemoryStream();
        var data = encoding.GetBytes(value);
        mStream.Write(data, 0, data.Length);
        mStream.Position = 0;
        return mStream;
    }
    /// <summary>
    /// 将字符串转化为内存字节流
    /// </summary>
    /// <param name="value">需转换的字符串</param>
    /// <param name="charset">字符集代码</param>
    /// <returns>字节流</returns>
    public static MemoryStream ToStream(this string value, string charset) => ToStream(value, Encoding.GetEncoding(charset));
    /// <summary>
    /// 将字符串以默认编码转化为内存字节流
    /// </summary>
    /// <param name="value">需转换的字符串</param>
    /// <returns>字节流</returns>
    public static MemoryStream ToStream(this string value) => ToStream(value, Encoding.UTF8);
    /// <summary>
    /// 将字符串拆分为数组
    /// </summary>
    /// <param name="value">需转换的字符串</param>
    /// <param name="separator">分割符</param>
    /// <returns>字符串数组</returns>
    public static string[] Split(this string value, string separator)
    {
        var collection = value.ToStringCollection(separator);
        var vs = new string[collection.Count];
        collection.CopyTo(vs, 0);
        return vs;
    }
    /// <summary>
    /// 将字符串转换为GUID
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid ToGuid(this string value) => new(value);
    #endregion

    #region Base64-String互转
    /// <summary>
    /// 将字符串转换成Base64字符串
    /// </summary>
    /// <param name="value">字符串</param>
    /// <returns></returns>
    public static string ToBase64(this string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    /// <summary>
    /// 将Base64字符转成String
    /// </summary>
    /// <param name="value">Base64字符串</param>
    /// <returns></returns>
    public static string Base64ToString(this string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
    #endregion

    #region 字符串插入指定分隔符
    /// <summary>
    /// 字符串插入指定分隔符
    /// </summary>
    /// <param name="text">字符串</param>
    /// <param name="spacingString">分隔符</param>
    /// <param name="spacingIndex">隔多少个字符插入分隔符</param>
    /// <returns></returns>
    public static string Spacing(this string text, string spacingString, int spacingIndex)
    {
        var sb = new StringBuilder(text);
        for (var i = spacingIndex; i <= sb.Length; i += spacingIndex + 1)
        {
            if (i >= sb.Length) break;
            _ = sb.Insert(i, spacingString);
        }
        return sb.ToString();
    }
    #endregion

    #region 半角全角相互转换
    /// <summary>
    /// 转全角的函数(SBC case)
    /// </summary>
    /// <param name="input">需要转换的字符串</param>
    /// <returns>转换为全角的字符串</returns>
    public static string ToSbc(this string input)
    {
        //半角转全角：
        var c = input.ToCharArray();
        for (var i = 0; i < c.Length; i++)
        {
            if (c[i] == 32)
            {
                c[i] = (char)12288;
                continue;
            }
            if (c[i] < 127) c[i] = (char)(c[i] + 65248);
        }
        return new(c);
    }

    /// <summary>
    ///  转半角的函数(SBC case)
    /// </summary>
    /// <param name="input">需要转换的字符串</param>
    /// <returns>转换为半角的字符串</returns>
    public static string ToDbc(this string input)
    {
        var c = input.ToCharArray();
        for (var i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }
            if (c[i] is > (char)65280 and < (char)65375)
            {
                c[i] = (char)(c[i] - 65248);
            }
        }
        return new(c);
    }
    #endregion

    #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证
    /// <summary>
    /// 检查一个字符串是否是纯数字构成的,一般用于查询字符串参数的有效性验证
    /// </summary>
    /// <param name="value">需验证的字符串</param>
    /// <returns>是否合法的bool值</returns>
    public static bool IsNumber(this string value) => QuickValidate(value, @"^\d+$");
    #endregion

    #region 快速验证一个字符串是否符合指定的正则表达式
    /// <summary>
    /// 快速验证一个字符串是否符合指定的正则表达式
    /// </summary>
    /// <param name="value">需验证的字符串</param>
    /// <param name="express">正则表达式的内容</param>
    /// <returns>是否合法的bool值</returns>
    public static bool QuickValidate(this string value, string express)
    {
        if (string.IsNullOrEmpty(value)) return false;
        var myRegex = new Regex(express);
        return myRegex.IsMatch(value);
    }
    #endregion

    #region 字符串反转
    /// <summary>
    /// 使用指针的方式反转字符串,该函数会修改原字符串.
    /// </summary>
    /// <param name="value">待反转字符串</param>
    /// <returns>反转后的结果</returns>
    public static unsafe string ReverseByPointer(this string value)
    {
        fixed (char* pText = value)
        {
            var pStart = pText;
            var pEnd = pText + value.Length - 1;
            while (pStart < pEnd)
            {
                var temp = *pStart;
                *pStart++ = *pEnd;
                *pEnd-- = temp;
            }
            return value;
        }
    }
    /// <summary>
    /// 使用StringBuilder和String索引器的方式反转字符串,该函数不会修改原字符串
    /// </summary>
    /// <param name="value">待反转字符串</param>
    /// <returns>反转后的结果</returns>
    public static string ReverseByStringBuilder(this string value)
    {
        var sb = new StringBuilder(capacity: value.Length);
        for (var i = value.Length; i > 0;)
        {
            _ = sb.Append(value[--i]);
        }
        return sb.ToString();
    }
    /// <summary>
    /// 使用Array.Reverse()的方式反转字符串,该函数不会修改原字符串
    /// </summary>
    /// <param name="value">待反转字符串</param>
    /// <returns>反转后的结果</returns>
    public static string ReverseByArray(this string value)
    {
        var arr = value.ToCharArray();
        Array.Reverse(arr);
        return new(arr);
    }
    #endregion
}