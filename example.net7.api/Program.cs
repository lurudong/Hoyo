using example.net7.api;
using FluentValidation;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Serilog;
using Serilog.Events;
using System.Reflection;
// 将输出日志格式化为ES需要的格式.
//using Serilog.Formatting.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// 配置支持HTTP1/2/3
//builder.WebHost.ConfigureKestrel((context, options) => options.ListenAnyIP(80, listenOptions =>
//{
//    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
//    //_ = listenOptions.UseHttps();
//}));

//添加Serilog配置
_ = builder.Host.UseSerilog((hbc, lc) =>
{
    _ = lc.ReadFrom.Configuration(hbc.Configuration).MinimumLevel.Override("Microsoft", LogEventLevel.Warning).MinimumLevel.Override("System", LogEventLevel.Warning).Enrich.FromLogContext();
    _ = lc.WriteTo.Async(wt => wt.Console(/*new ElasticsearchJsonFormatter()*/));
    _ = lc.WriteTo.Debug();
    //_ = lc.WriteTo.MongoDB(hbc.Configuration["Logging:DataBase:Mongo"]);
    // 不建议将日志写入文件,会造成日志文件越来越大,服务器可能因此宕机.
    // 若是需要分文件写入需要引入包 Serilog.Sinks.Map
    //_ = lc.WriteTo.Map(le =>
    //{
    //    static (DateTime time, LogEventLevel level) MapData(LogEvent @event) => (@event.Timestamp.LocalDateTime, @event.Level);
    //    return MapData(le);
    //}, (key, log) =>
    //{
    //    log.Async(o => o.File(Path.Combine("logs", @$"{key.time:yyyyMMdd}{Path.DirectorySeparatorChar}{key.level.ToString().ToLower()}.log"), logEventLevel));
    //});
});

// Add services to the container.
// 自动注入服务模块
builder.Services.AddApplication<AppWebModule>();

//FluentValidation.DependencyInjectionExtensions 
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

// 添加自动化注入的一些中间件.
app.InitializeApplication();
app.MapControllers();

app.Run();