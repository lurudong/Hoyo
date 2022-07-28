using example.net7.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// 配置支持HTTP1/2/3
builder.WebHost.ConfigureKestrel((context, options) => options.ListenAnyIP(5273, listenOptions =>
{
    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    _ = listenOptions.UseHttps();
}));

//添加SeriLog配置
_ = builder.Host.UseSerilog((webHost, logconfig) =>
{
    var configuration = webHost.Configuration.GetSection("Serilog");
    var minilevel = string.IsNullOrWhiteSpace(configuration.Value) ? LogEventLevel.Information.ToString() : configuration["MinimumLevel:Default"]!;
    //日志事件级别
    var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minilevel);
    _ = logconfig.ReadFrom.Configuration(configuration).Enrich.FromLogContext().WriteTo.Console(logEventLevel);
    // 若是需要分文件写入需要引入包 Serilog.Sinks.Map
    //_ = logconfig.WriteTo.Map(le => MapData(le), (key, log) => log.Async(o => o.File(Path.Combine("logs", @$"{key.time:yyyyMMdd}{Path.DirectorySeparatorChar}{key.level.ToString().ToLower()}.log"), logEventLevel)));
    //static (DateTime time, LogEventLevel level) MapData(LogEvent @event) => (@event.Timestamp.LocalDateTime, @event.Level);
}).ConfigureLogging((hostcontext, builder) => builder.ClearProviders().SetMinimumLevel(LogLevel.Information).AddConfiguration(hostcontext.Configuration.GetSection("Logging")).AddConsole().AddDebug());
// Add services to the container.
// 自动注入服务模块
builder.Services.AddApplication<AppWebModule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

// 添加自动化注入的一些中间件.
app.InitializeApplication();
app.MapControllers();

app.Run();
