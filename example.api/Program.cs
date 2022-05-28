using example.api;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.GridFS;
using Hoyo.WebCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(c =>
{
    _ = c.Filters.Add<ActionExecuteFilter>();
    _ = c.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(c =>
{
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyNullJsonConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyNullJsonConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
    c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 自动注入服务模块
builder.Services.AddApplication<AppWebModule>();

var dboptions = new HoyoMongoOptions();
dboptions.AppendConventionRegistry("IdentityServer Mongo Conventions", new()
{
    Conventions = new()
    {
        new IgnoreExtraElementsConvention(true)
    },
    Filter = _ => true
});

var db = await builder.Services.AddMongoDbContext<DbContext>(clientSettings: new()
{
    ServerAddresses = new()
    {
        new("192.168.2.10", 27017),
    },
    AuthDatabase = "admin",
    DatabaseName = "miracle",
    UserName = "oneblogs",
    Password = "&oneblogs.cn",
}, dboptions: dboptions);

var fsop = new HoyoGridFSOptions()
{
    BusinessApp = "HoyoFS",
    Options = new()
    {
        BucketName = "hoyo",
        ChunkSizeBytes = 1024,
        DisableMD5 = true,
        ReadConcern = new() { },
        ReadPreference = ReadPreference.Primary,
        WriteConcern = WriteConcern.Unacknowledged
    },
    DefalutDB = true,
    ItemInfo = "item.info"
};

builder.Services.AddHoyoGridFS(db._database!, fsop);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

app.UseHoyoResponseTime();
app.UseAuthorization();

app.MapControllers();

// 添加自动化注入的一些中间件.
app.InitializeApplication();

app.Run();
