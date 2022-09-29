using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.WebCore;
using System.Text.Json.Serialization;

namespace example.net7.api;

/// <summary>
/// 注册一些控制器的基本内容
/// </summary>
public class ControllersModule : AppModule
{
    /// <summary>
    /// 注册和配置服务
    /// </summary>
    /// <param name="context"></param>
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
