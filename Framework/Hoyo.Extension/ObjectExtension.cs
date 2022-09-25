﻿using System.Reflection;

namespace Hoyo.Extensions;

public static class ObjectExtension
{
    /// <summary>
    /// 验证指定值的断言<paramref name="assertion"/>是否为真，如果不为真，抛出指定消息<paramref name="message"/>的指定类型<typeparamref name="TException"/>异常
    /// </summary>
    /// <typeparam name="TException">异常类型</typeparam>
    /// <param name="assertion">要验证的断言。</param>
    /// <param name="message">异常消息。</param>
    public static void Require<TException>(bool assertion, string message) where TException : Exception
    {
        if (assertion) return;
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));
        throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }

    /// <summary>
    /// 验证指定值的断言表达式是否为真，不为值抛出<see cref="Exception"/>异常
    /// </summary>
    /// <param name="value"></param>
    /// <param name="assertionFunc">要验证的断言表达式</param>
    /// <param name="message">异常消息</param>
    public static void Required<T>(this T value, Func<T, bool> assertionFunc, string message)
    {
        if (assertionFunc is null) throw new ArgumentNullException(nameof(assertionFunc));
        Require<Exception>(assertionFunc(value), message);
    }

    /// <summary>
    /// 验证指定值的断言表达式是否为真，不为真抛出<typeparamref name="TException"/>异常
    /// </summary>
    /// <typeparam name="T">要判断的值的类型</typeparam>
    /// <typeparam name="TException">抛出的异常类型</typeparam>
    /// <param name="value">要判断的值</param>
    /// <param name="assertionFunc">要验证的断言表达式</param>
    /// <param name="message">异常消息</param>
    public static void Required<T, TException>(this T value, Func<T, bool> assertionFunc, string message) where TException : Exception
    {
        if (assertionFunc is null) throw new ArgumentNullException(nameof(assertionFunc));
        Require<TException>(assertionFunc(value), message);
    }

    /// <summary>
    /// 检查参数不能为空引用，否则抛出<see cref="ArgumentNullException"/>异常。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName">参数名称</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNull<T>(this T value, string paramName) => Require<ArgumentNullException>(value is not null, $"参数“{paramName}”不能为空引用。");

    /// <summary>
    /// 检查字符串不能为空引用或空字符串，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName">参数名称。</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void NotNullOrEmpty(this string value, string paramName) => Require<ArgumentException>(!string.IsNullOrEmpty(value), $"参数“{paramName}”不能为空引用或空字符串。");

    /// <summary>
    /// 检查Guid值不能为Guid.Empty，否则抛出<see cref="ArgumentException"/>异常。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="paramName">参数名称。</param>
    /// <exception cref="ArgumentException"></exception>
    public static void NotEmpty(this Guid value, string paramName) => Require<ArgumentException>(value != Guid.Empty, $"参数“{paramName}”的值不能为Guid.Empty");

    /// <summary>
    /// 检查集合不能为空引用或空集合，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
    /// </summary>
    /// <typeparam name="T">集合项的类型。</typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName">参数名称。</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void NotNullOrEmpty<T>(this IEnumerable<T> collection, string paramName) => Require<ArgumentException>(collection.Any(), $"参数“{paramName}”不能为空引用或空集合。");

    /// <summary>
    /// 判断特性相应是否存在
    /// </summary>
    /// <typeparam name="T">动态类型要判断的特性</typeparam>
    /// <param name="memberInfo"></param>
    /// <param name="inherit"></param>
    /// <returns>如果存在还在返回true，否则返回false</returns>
    public static bool HasAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute => memberInfo.IsDefined(typeof(T), inherit);
}
