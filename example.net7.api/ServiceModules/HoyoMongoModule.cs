using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.Extension;
using MongoDB.Bson.Serialization.Conventions;

namespace example.net7.api;

public class HoyoMongoModule : AppModule
{
    public HoyoMongoModule()
    {
        Enable = false;
    }
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var config = context.Services.GetConfiguration();

        _ = context.Services.AddTypeExtension().AddMongoDbContext<DbContext>(config, dboptions: op =>
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
            op.UseDefalutConventionRegistryConfig = true;
        }).AddMongoDbContext<DbContext2>(config, dboptions: op =>
        {
            op.RegistryPackFirst = false;
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
            op.UseDefalutConventionRegistryConfig = true;
        });
    }
}
