using example.local.api;
using Hoyo.Mongo;
using Hoyo.Mongo.GridFS;
using Hoyo.Mongo.GridFS.Extension;
using Hoyo.WebCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "example.local.api", Version = "v1" }));
builder.Services.AddCors(c => c.AddPolicy("AllowedHosts", s => s.WithOrigins(builder.Configuration["AllowedHosts"].Split(",")).AllowAnyMethod().AllowAnyHeader()));

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

builder.Services.AddMiracleGridFS(db._database!, fsop);

builder.Services.AddControllers(c =>
{
    c.Filters.Add<ActionExecuteFilter>();
    c.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(c =>
{
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
    c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
    c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) _ = app.UseDeveloperExceptionPage();

app.UseHoyoResponseTime();
app.UseCors("AllowedHosts");

app.UseAuthorization();

app.UseHoyoGridFSVirtualPath(builder.Configuration);

app.MapControllers();
app.UseSwagger().UseSwaggerUI();

app.Run();