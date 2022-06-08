using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.GridFS;
using Hoyo.Mongo.GridFS.Extension;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace example.api;

public class HoyoMongoModule : AppModule
{
    public override async void ConfigureServices(ConfigureServicesContext context)
    {
        var dboptions = new HoyoMongoOptions();
        dboptions.AppendConventionRegistry("IdentityServer Mongo Conventions", new()
        {
            Conventions = new()
            {
                new IgnoreIfDefaultConvention(true)
            },
            Filter = _ => true
        });

        var db = await context.Services.AddMongoDbContext<DbContext>(clientSettings: new()
        {
            ServerAddresses = new()
            {
                new("192.168.2.10", 27017),
            },
            AuthDatabase = "admin",
            DatabaseName = "hoyo",
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
        _ = context.Services.AddHoyoGridFS(db._database!, fsop);
    }

    public override void ApplicationInitialization(ApplicationContext context)
    {
        var app = context.GetApplicationBuilder();
        var config = app.ApplicationServices.GetService<IConfiguration>() ?? throw new("Configuration Service can't be null");
        // 添加虚拟目录用于缓存文件,便于在线播放等功能.
        _ = app.UseHoyoGridFSVirtualPath(config);
    }
}
