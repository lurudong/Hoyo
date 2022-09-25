using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Hoyo.IdentityServer.Storage.MongoDB;

public static class RepositoryExtensions
{
    public static IIdentityServerBuilder AddMongoRepository(this IIdentityServerBuilder builder, string? dbName = null)
    {
        var db = builder.Services.BuildServiceProvider().GetService<IMongoDatabase>() ?? throw new NullReferenceException("mongo database not found!");
        if (dbName is not null)
        {
            var client = builder.Services.BuildServiceProvider().GetService<IMongoClient>() ?? throw new NullReferenceException("mongo client not found!");
            db = client.GetDatabase(dbName);
        }
        _ = builder.Services.AddTransient<IRepository, MongoRepository>(_ => new(db));
        return builder;
    }

    public static IIdentityServerBuilder AddIdentityClients(this IIdentityServerBuilder builder)
    {
        _ = builder.Services.AddTransient<IClientStore, RepositoryClientStore>();
        return builder;
    }

    public static IIdentityServerBuilder AddIdentityResources(this IIdentityServerBuilder builder)
    {
        _ = builder.Services.AddTransient<IResourceStore, RepositoryResourceStore>();
        return builder;
    }

    public static IIdentityServerBuilder AddIdentityPersistedGrants(this IIdentityServerBuilder builder)
    {
        _ = builder.Services.AddTransient<IPersistedGrantStore, RepositoryPersistedGrantStore>();
        return builder;
    }

    public static IIdentityServerBuilder AddPolicyService(this IIdentityServerBuilder builder)
    {
        _ = builder.Services.AddSingleton<ICorsPolicyService, RepositoryCorsPolicyService>();
        return builder;
    }
}
