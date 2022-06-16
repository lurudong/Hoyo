using eventbus.events;
using Hoyo.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace eventbus.publish.api.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    public void PublishEventMessage()
    {
        string userid = Guid.NewGuid().ToString();
        var eventMessage = new OrderStartedIntegrationEvent(userid);
        _eventBus.Publish("order.publish.test",eventMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("[action]")]
    public void PublishSecond()
    {
        string userid = "san";
        var eventMessage = new TestSecondEvent(userid);
        _eventBus.Publish("second.publish.test", eventMessage);
    }
}
