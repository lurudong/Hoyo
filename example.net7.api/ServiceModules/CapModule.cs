using Hoyo.AutoDependencyInjectionModule.Modules;

namespace example.net7.api;

public class CapModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        _ = context.Services.AddCap(c =>
        {
            c.DefaultGroupName = "hoyo.cap";
            _ = c.UseMongoDB(s =>
            {
                s.DatabaseConnection = "mongodb://a:123456@127.0.0.1:27017/?authSource=admin&serverSelectionTimeoutMS=1000";
                s.DatabaseName = "hoyocap";
                s.PublishedCollection = "hoyo.cap.published";
                s.ReceivedCollection = "hoyo.cap.received";
            });
            _ = c.UseRabbitMQ(s =>
            {
                s.HostName = "192.168.2.35";
                s.Port = 5672;
                s.UserName = "admin";
                s.Password = "&789";
                s.ExchangeName = "hoyo.default.cap";
                s.VirtualHost = "/";
            });
        });
    }
}
