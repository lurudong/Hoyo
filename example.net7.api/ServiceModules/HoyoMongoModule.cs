using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using Hoyo.Mongo.Extension;
using MongoDB.Bson.Serialization.Conventions;

namespace example.net7.api;

public class HoyoMongoModule : AppModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
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

        _ = context.Services.AddMongoDbContext<DbContext>(clientSettings: new()
        {
            ServerAddresses = new()
            {
                new("101.34.26.221", 40003),
            },
            AuthDatabase = "admin",
            DatabaseName = "hoyo",
            UserName = "oneblogs",
            Password = "&oneblogs789",
        }, dboptions: dboptions).AddTypeExtension();
    }
}
