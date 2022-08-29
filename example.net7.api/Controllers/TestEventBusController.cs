using example.net7.api.EventHandlers;
using Hoyo.EventBus;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;

[ApiController, Route("[controller]")]
public class TestEventBusController : ControllerBase
{
    private readonly IIntegrationEventBus _integrationEventBus;

    public TestEventBusController(IIntegrationEventBus integrationEventBus)
    {
        _integrationEventBus = integrationEventBus;
    }

    [HttpPost]
    public void CreateOrder()
    {
        var orderEvent = new CreateOrderEvent() { Message = "test0000000001" };
        _integrationEventBus.Publish<CreateOrderEvent>(orderEvent);
    }

    [HttpGet]
    public void CreateTestEvent()
    {
        var test = new TestEvent() { Message = "大黄瓜1CM，真的猛" };
        _integrationEventBus.Publish<TestEvent>(test);
    }
    [HttpPost("TTLTest")]
    public void TTLTest()
    {
        var ttlobj = new DelayedMessageEvent() { Message = "大黄瓜0.5cm,猛不起来了" };
        _integrationEventBus.PublishWithTTL<DelayedMessageEvent>(ttlobj, 5000);
    }
}
