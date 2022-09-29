using example.net7.api.EventHandlers;
using Hoyo.EventBus;
using Hoyo.WebCore.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;

/// <summary>
/// 消息总线测试接口
/// </summary>
[ApiController, Route("api/[controller]")]
[ApiGroup("TestEventBus", "2022-09-29", "测试消息总线")]
public class TestEventBusController : ControllerBase
{
    private readonly IIntegrationEventBus _integrationEventBus;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="integrationEventBus"></param>
    public TestEventBusController(IIntegrationEventBus integrationEventBus)
    {
        _integrationEventBus = integrationEventBus;
    }
    /// <summary>
    /// 创建一个普通消息
    /// </summary>
    [HttpPost]
    public void CreateOrder()
    {
        var orderEvent = new TestEvent() { Message = "大黄瓜1CM，真的猛" };
        _integrationEventBus.Publish(orderEvent);
    }
    /// <summary>
    /// 创建一个延时消息,同时发送一个普通消息做对比
    /// </summary>
    [HttpPost("TTLTest")]
    public void TTLTest()
    {
        var rand = new Random();
        var ttl = rand.Next(1000, 10000);
        var ttlobj = new DelayedMessageEvent() { Message = $"延迟{ttl}毫秒,当前时间{DateTime.Now:yyyy-MM-dd HH:mm:ss},猛不起来了" };
        _integrationEventBus.Publish(ttlobj, (uint)ttl);
        _integrationEventBus.Publish(ttlobj);
    }
    /// <summary>
    /// 使用特性的方式发送延时消息,同时测试传入ttl后对默认标记的影响.
    /// </summary>
    [HttpPost("TTLTest_1")]
    public void TTLTest_1()
    {
        var ttlobj = new DelayedMessageEvent2() { Message = $"大黄瓜0.5cm,当前时间{DateTime.Now:yyyy-MM-dd HH:mm:ss},猛不起来了" };
        _integrationEventBus.Publish(ttlobj, 3000);
        _integrationEventBus.Publish(ttlobj);
        _integrationEventBus.Publish(ttlobj, 7000);
    }
}
