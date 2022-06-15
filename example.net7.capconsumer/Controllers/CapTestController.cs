using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.capconsumer.Controllers;
[Route("[controller]")]
[ApiController]
public class CapTestController : ControllerBase
{
    [NonAction]
    [CapSubscribe("sample.rabbitmq.captest")]
    public void ReceiveMessage(DateTime time)
    {
        Console.WriteLine("message time is:" + time);
    }
}
