using example.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.WebCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(c =>
{
    c.Filters.Add(new ActionExecuteFilter());
    c.Filters.Add(new ExceptionFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 自动注入服务模块
builder.Services.AddApplication<AppWebModule>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

app.UseHoyoResponseTime();
app.UseAuthorization();

app.MapControllers();

// 添加自动化注入的一些中间件.
app.InitializeApplication();

app.Run();
