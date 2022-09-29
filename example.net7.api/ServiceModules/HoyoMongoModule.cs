using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.Extension;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace example.net7.api;
/// <summary>
/// MongoDB驱动模块
/// </summary>
public class HoyoMongoModule : AppModule
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public HoyoMongoModule()
    {
        Enable = false;
    }
    /// <summary>
    /// 配置和注册服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddMongoDbContext<DbContext>("mongodb://", options: op =>
        {
            op.AppendConventionRegistry(new()
            {
                {
                    "IdentityServer Mongo Conventions",
                    new()
                    {
                        new IgnoreIfDefaultConvention(true)
                    }
                }
            });
            op.DefaultConventionRegistry = true;
        }).AddMongoDbContext<DbContext2>("mongodb://", options: op =>
        {
            op.DefaultConventionRegistry = true;
            op.AppendConventionRegistry(new()
            {
                {
                    "IdentityServer Mongo Conventions",
                    new()
                    {
                        new IgnoreIfDefaultConvention(true)
                    }
                }
            });
            op.ObjectIdToStringTypes = new() { typeof(MongoTest) };
        }).RegisterHoyoSerializer();
        _ = context.Services.RegisterHoyoSerializer(new DoubleSerializer(BsonType.Double));
    }
}
