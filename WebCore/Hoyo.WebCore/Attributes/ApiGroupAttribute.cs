namespace Hoyo.WebCore.Attributes;

/// <summary>
/// 被此特性标记的控制器可在Swagger文档分组中发挥作用.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ApiGroupAttribute : Attribute
{
    public ApiGroupAttribute(string title, string version, string description = "")
    {
        Title = title;
        Version = version;
        Description = description;
    }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}
