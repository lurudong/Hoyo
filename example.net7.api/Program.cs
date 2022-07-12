using example.net7.api;
using Hoyo.AutoDependencyInjectionModule.Modules;

var builder = WebApplication.CreateBuilder(args);

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
