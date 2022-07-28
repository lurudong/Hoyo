using example.net7.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// ����֧��HTTP1/2/3
builder.WebHost.ConfigureKestrel((context, options) => options.ListenAnyIP(5273, listenOptions =>
{
    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    _ = listenOptions.UseHttps();
}));

//���SeriLog����
_ = builder.Host.UseSerilog((webHost, logconfig) =>
{
    var configuration = webHost.Configuration.GetSection("Serilog");
    var minilevel = string.IsNullOrWhiteSpace(configuration.Value) ? LogEventLevel.Information.ToString() : configuration["MinimumLevel:Default"]!;
    //��־�¼�����
    var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minilevel);
    _ = logconfig.ReadFrom.Configuration(configuration).Enrich.FromLogContext().WriteTo.Console(logEventLevel);
    // ������Ҫ���ļ�д����Ҫ����� Serilog.Sinks.Map
    //_ = logconfig.WriteTo.Map(le => MapData(le), (key, log) => log.Async(o => o.File(Path.Combine("logs", @$"{key.time:yyyyMMdd}{Path.DirectorySeparatorChar}{key.level.ToString().ToLower()}.log"), logEventLevel)));
    //static (DateTime time, LogEventLevel level) MapData(LogEvent @event) => (@event.Timestamp.LocalDateTime, @event.Level);
}).ConfigureLogging((hostcontext, builder) => builder.ClearProviders().SetMinimumLevel(LogLevel.Information).AddConfiguration(hostcontext.Configuration.GetSection("Logging")).AddConsole().AddDebug());
// Add services to the container.
// �Զ�ע�����ģ��
builder.Services.AddApplication<AppWebModule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

// ����Զ���ע���һЩ�м��.
app.InitializeApplication();
app.MapControllers();

app.Run();
