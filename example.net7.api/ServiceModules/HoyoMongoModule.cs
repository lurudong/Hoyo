using Hoyo.AutoDependencyInjectionModule.Modules;
using Hoyo.Mongo;
using MongoDB.Bson.Serialization.Conventions;

namespace example.net7.api;

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
                new("101.34.26.221", 40003),
            },
            AuthDatabase = "admin",
            DatabaseName = "hoyo",
            UserName = "oneblogs",
            Password = "&duyu789",
        }, dboptions: dboptions);
    }
}
