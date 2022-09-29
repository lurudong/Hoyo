﻿namespace Hoyo.Universal;

/// <summary>
/// 操作人
/// </summary>
public class Operator : ReferenceItem
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="rid">相关ID</param>
    /// <param name="name">名称</param>
    public Operator(string rid, string name) : base(rid, name) { }
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime? Time { get; set; }
}