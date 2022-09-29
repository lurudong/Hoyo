using System.Text.Json.Serialization;

namespace Hoyo.EventBus;

/// <summary>
/// 事件基本对象,所有的事件均需要继承此类
/// </summary>
public class IntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid().ToString();
        EventCreationDate = DateTime.Now;
    }
    /// <summary>
    /// 事件ID
    /// </summary>
    [JsonInclude]
    public string EventId { get; }
    /// <summary>
    /// 事件创建时间
    /// </summary>
    [JsonInclude]
    public DateTime EventCreationDate { get; }
}