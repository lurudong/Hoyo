using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo.GridFS;
using Hoyo.Mongo.GridFS.Extension;
using MongoDB.Driver;

namespace example.net7.api;

/// <summary>
/// MongoDBGridFS模块
/// </summary>
public class HoyoMongoGridFSModule : AppModule
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public HoyoMongoGridFSModule()
    {
        Enable = false;
    }
    /// <summary>
    /// 注册和配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var db = context.Services.GetService<DbContext>() ?? throw new("MongoDB数据库服务不存在,若是不适用默认的数据库服务,可以创建新的数据库链接来使用GridFS");

        _ = context.Services.AddHoyoGridFS(db.Database, op =>
        {
            op.BusinessApp = "HoyoFS";
            op.Options = new()
            {
                BucketName = "hoyo",
                ChunkSizeBytes = 1024,
                DisableMD5 = true,
                ReadConcern = new(),
                ReadPreference = ReadPreference.Primary,
                WriteConcern = WriteConcern.Unacknowledged
            };
            op.DefaultDB = true;
            op.ItemInfo = "item.info";
        });
    }
    /// <summary>
    /// 注册中间件
    /// </summary>
    /// <param name="context"></param>
    public override void ApplicationInitialization(ApplicationContext context)
    {
        var app = context.GetApplicationBuilder();
        var config = app.ApplicationServices.GetService<IConfiguration>() ?? throw new("Configuration Service can't be null");
        // 添加虚拟目录用于缓存文件,便于在线播放等功能.
        _ = app.UseHoyoGridFSVirtualPath(config);
    }
}
