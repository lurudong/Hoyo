﻿using System.ComponentModel;

namespace Hoyo.EventBus.RabbitMQ.Enums;
public enum EExchange
{
    /// <summary>
    /// 路由模式
    /// </summary>
    [Description("direct")]
    Routing,
    /// <summary>
    /// 发布/订阅模式
    /// </summary>
    [Description("fanout")]
    Publish,
    /// <summary>
    /// 主题模式
    /// </summary>
    [Description("topic")]
    Topic,
    /// <summary>
    /// 延时x-delayed-message模式
    /// </summary>
    [Description("x-delayed-message")]
    Delayed
}
