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
    }
}
