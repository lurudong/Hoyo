using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;
[Route("[controller]")]
[ApiController]
public class PublishController : ControllerBase
{
    private readonly ICapPublisher _capBus;

    public PublishController(ICapPublisher capPublisher)
    {
        _capBus = capPublisher;
    }

    [HttpGet("Publish")]
    public void Publish()
    {
        _capBus.Publish("sample.rabbitmq.captest", DateTime.Now);
        _capBus.Publish("sample.rabbitmq.captestinservice", DateOnly.FromDateTime(DateTime.Now));
    }
}
