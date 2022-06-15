using DotNetCore.CAP;

namespace example.net7.capconsumer;
public interface ISubscriberService
{
    void CheckReceivedMessage(DateOnly datetime);
}
public class CapInService : ISubscriberService, ICapSubscribe
{
    [CapSubscribe("sample.rabbitmq.captestinservice")]
    public virtual void CheckReceivedMessage(DateOnly datetime)
    {
        Console.WriteLine("message in service time is:" + datetime);
    }
}
