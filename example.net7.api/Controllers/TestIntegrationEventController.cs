﻿using example.net7.api.EventHandlers;
using Hoyo.EventBus;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;

[ApiController, Route("[controller]")]
public class TestIntegrationEventController : ControllerBase
{
    private readonly IIntegrationEventBus _integrationEventBus;

    public TestIntegrationEventController(IIntegrationEventBus integrationEventBus)
    {
        _integrationEventBus = integrationEventBus;
    }

    [HttpPost]
    public void CreateOrder()
    {
        var orderEvent = new CreateOrderIntegrationEvent() { OrderNo = "test0000000001" };
        _integrationEventBus.Publish<CreateOrderIntegrationEvent>(orderEvent);

    }

    [HttpGet]
    public void CreateTestIntegrationEvent()
    {
        var test = new TestIntegrationEvent() { Name="大黄瓜18CM，真的猛" };
        _integrationEventBus.Publish<TestIntegrationEvent>(test);

    }
}
