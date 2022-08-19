using Hoyo.AutoDependencyInjectionModule.Extensions;
using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.Extension;
using MongoDB.Bson.Serialization.Conventions;

namespace example.net7.api;

public class HoyoMongoModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        var dboptions = new HoyoMongoOptions()
        {
            //UseDefalutConventionRegistryConfig = false,
        };
        dboptions.AppendConventionRegistry(new()
        {
            {
                "IdentityServer Mongo Conventions",
                new()
                {
                   new IgnoreIfDefaultConvention(true)
                }
            }
        });

        var config = context.Services.GetConfiguration();

        _ = context.Services.AddTypeExtension().AddMongoDbContext<DbContext>(config, dboptions: dboptions);
    }
}
