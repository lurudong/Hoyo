using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.net7.capconsumer;

public class CapModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        _ = context.Services.AddCap(c =>
        {
            c.DefaultGroupName = "hoyo.cap";
            _ = c.UseMongoDB(s =>
            {
                s.DatabaseConnection = "mongodb://bl:a123456@altzyxy.com:27010/?authSource=admin&serverSelectionTimeoutMS=1000";
                s.DatabaseName = "hoyocap";
                s.PublishedCollection = "hoyo.cap.published";
                s.ReceivedCollection = "hoyo.cap.received";
            });
            _ = c.UseRabbitMQ(s =>
            {
                s.HostName = "101.34.26.221";
                s.Port = 40013;
                s.UserName = "admin";
                s.Password = "&duyu789";
                s.ExchangeName = "hoyo.default.cap";
                s.VirtualHost = "/";
            });
        });

        // TODO: 当消费者服务未注册,或者程序挂掉后.未处理数据.再次启动消费者后,无法消费之前的消息,从rabbitmq-ui中看也不存在消息,难不成用CAP要保证程序不会挂掉,程序员也不能写错代码?
        _ = context.Services.AddTransient<ISubscriberService, CapInService>();
    }
}
