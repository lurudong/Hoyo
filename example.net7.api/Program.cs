using example.net7.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Serilog;
using Serilog.Events;
// �������־��ʽ��ΪES��Ҫ�ĸ�ʽ.
//using Serilog.Formatting.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// ����֧��HTTP1/2/3
//builder.WebHost.ConfigureKestrel((context, options) => options.ListenAnyIP(80, listenOptions =>
//{
//    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
//    //_ = listenOptions.UseHttps();
//}));

//���SeriLog����
_ = builder.Host.UseSerilog((hbc, lc) =>
{
    _ = lc.ReadFrom.Configuration(hbc.Configuration).MinimumLevel.Override("Microsoft", LogEventLevel.Warning).MinimumLevel.Override("System", LogEventLevel.Warning).Enrich.FromLogContext();
    _ = lc.WriteTo.Async(wt => wt.Console(/*new ElasticsearchJsonFormatter()*/));
    _ = lc.WriteTo.Debug();
    //_ = lc.WriteTo.MongoDB(hbc.Configuration["Logging:DataBase:Mongo"]);
    // �����齫��־д���ļ�,�������־�ļ�Խ��Խ��,�������������崻�.
    // ������Ҫ���ļ�д����Ҫ����� Serilog.Sinks.Map
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
// �Զ�ע�����ģ��
builder.Services.AddApplication<AppWebModule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

// ����Զ���ע���һЩ�м��.
app.InitializeApplication();
app.MapControllers();

app.Run();
