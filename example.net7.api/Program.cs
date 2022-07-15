using example.net7.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// 配置支持HTTP1/2/3
builder.WebHost.ConfigureKestrel((context, options) => options.ListenAnyIP(5273, listenOptions =>
{
    listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
    _ = listenOptions.UseHttps();
}));

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
