using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Serilog;
using Serilog.Events;

namespace example.net7.api;

public class SerilogModule : AppModule
{
    public SerilogModule()
    {
        Enable = false;
    }
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        //添加SeriLog配置
        var config = context.Services.GetConfiguration();
        var configuration = config.GetSection("Serilog");
        var minilevel = string.IsNullOrWhiteSpace(configuration.Value) ? LogEventLevel.Information.ToString() : configuration["MinimumLevel:Default"]!;
        //日志事件级别
        var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minilevel);

        _ = context.Services.AddLogging(builder =>
        {
            _ = builder.ClearProviders().SetMinimumLevel(LogLevel.Information).AddConfiguration(config.GetSection("Logging")).AddConsole().AddDebug();
            //static (DateTime time, LogEventLevel level) MapData(LogEvent @event) => (@event.Timestamp.LocalDateTime, @event.Level);
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
               .Enrich.FromLogContext().WriteTo.Console(logEventLevel)
               // 若是分文件写,需要引入Serilog.Sinks.Map 与 Serilog.Sinks.Async
               //.WriteTo.Map(le => MapData(le), (key, log) => log.Async(o => o.File(Path.Combine("logs", @$"{key.time:yyyyMMdd}{Path.DirectorySeparatorChar}{key.level.ToString().ToLower()}.log"), logEventLevel)))
               .CreateLogger();

            _ = builder.AddSerilog();
        });
    }
}
