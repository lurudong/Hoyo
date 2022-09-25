using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.Extension;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace example.net7.api;

public class HoyoMongoModule : AppModule
{
    public HoyoMongoModule()
    {
        Enable = !false;
    }
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var config = context.Services.GetConfiguration();

        context.Services.AddMongoDbContext<DbContext>("mongodb://bl:a123456@altzyxy.com:27010/test1?authSource=admin&serverSelectionTimeoutMS=1000", options: op =>
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
        }).AddMongoDbContext<DbContext2>("mongodb://bl:a123456@altzyxy.com:27010/test2?authSource=admin&serverSelectionTimeoutMS=1000", options: op =>
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
