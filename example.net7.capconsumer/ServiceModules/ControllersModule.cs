using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.WebCore;
using System.Text.Json.Serialization;

namespace example.net7.capconsumer;

public class ControllersModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        _ = context.Services.AddControllers(x =>
        {
            _ = x.Filters.Add<ActionExecuteFilter>();
            _ = x.Filters.Add<ExceptionFilter>();
        }).AddJsonOptions(c =>
        {
            c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
            c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
            c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyNullJsonConverter());
            c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyNullJsonConverter());
            c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
            c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        _ = context.Services.AddEndpointsApiExplorer();
    }
}
