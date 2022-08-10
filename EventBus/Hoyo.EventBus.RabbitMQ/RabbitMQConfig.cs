namespace Hoyo.EventBus.RabbitMQ;

public class RabbitMQConfig
{
    public string Host { get; set; } = default!;

    public string PassWord { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public int RetryCount { get; set; } = default!;

    public int Port { get; set; } = default!;

    public string VirtualHost { get; set; } = "/";
}