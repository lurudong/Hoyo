using Hoyo.AutoDependencyInjectionModule.Modules;
using WebApplication1;
// 将输出日志格式化为ES需要的格式.
//using Serilog.Formatting.Elasticsearch;

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
