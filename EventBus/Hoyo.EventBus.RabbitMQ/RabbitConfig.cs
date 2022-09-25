namespace Hoyo.EventBus.RabbitMQ;

public class RabbitConfig
{
    public string Host { get; set; } = "localhost";
    public string PassWord { get; set; } = "guest";
    public string UserName { get; set; } = "guest";
    public int RetryCount { get; set; } = 5;
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
}