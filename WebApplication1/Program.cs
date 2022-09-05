using Hoyo.AutoDependencyInjectionModule.Modules;
using WebApplication1;
// �������־��ʽ��ΪES��Ҫ�ĸ�ʽ.
//using Serilog.Formatting.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

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
