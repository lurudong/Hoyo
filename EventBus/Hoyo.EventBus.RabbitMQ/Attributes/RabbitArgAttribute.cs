﻿namespace Hoyo.EventBus.RabbitMQ.Attributes;

/// <summary>
/// RabbitMQ参数特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RabbitArgAttribute : RabbitDictionaryAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public RabbitArgAttribute(string key, object value) : base(key, value) { }
}
