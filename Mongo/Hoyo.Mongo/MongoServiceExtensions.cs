using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hoyo.Mongo;
/// <summary>
/// 1.Create a DbContext use connectionString with [ConnectionStrings.Mongo in appsettings.json] or with [CONNECTIONSTRINGS_MONGO] setting value in environment variable
/// 2.Inject DbContext use services.AddSingleton(db);
/// 3.Inject IMongoDataBase use services.AddSingleton(db._database);
/// </summary>
public static class MongoServiceExtensions
{
    /// <summary>
    /// 获取连接字符串,并提供一些信息输出
    /// </summary>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="connKey">链接字符串的环境变量或者配置名</param>
    /// <returns></returns>
    private static string ConnectionString(IConfiguration configuration, string connKey = "CONNECTIONSTRINGS_MONGO")
    {
        var connectionString = configuration[connKey];
        if (string.IsNullOrWhiteSpace(connectionString)) connectionString = configuration.GetConnectionString("Mongo");
        return string.IsNullOrWhiteSpace(connectionString)
            ? throw new("💔:无 [CONNECTIONSTRINGS_MONGO] 系统环境变量或appsetting.json中不存在ConnectionStrings:Mongo配置")
            : connectionString;
    }

    /// <summary>
    /// 通过连接字符串添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="dboptions">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, IConfiguration configuration, string connKey = "CONNECTIONSTRINGS_MONGO", HoyoMongoOptions? dboptions = null) where T : BaseDbContext
    {
        dboptions ??= new();
        var connectionString = ConnectionString(configuration, connKey);
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(connectionString);
        db.BuildTransactCollections();
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database!);
        return services;
    }

    /// <summary>
    /// 使用HoyoMongoClientSettings配置添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="clientSettings">HoyoMongoClientSettings</param>
    /// <param name="dboptions">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, HoyoMongoClientSettings clientSettings, HoyoMongoOptions? dboptions = null) where T : BaseDbContext
    {
        dboptions ??= new();
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(clientSettings);
        db.BuildTransactCollections();
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database!);
        return services;
    }

    /// <summary>
    /// 通过连接字符串添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.IDbSet</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="dboptions">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbSet<T>(this IServiceCollection services, IConfiguration configuration, HoyoMongoOptions? dboptions = null) where T : BaseDbContext, IDbSet
    {
        dboptions ??= new();
        var connectionString = ConnectionString(configuration);
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(connectionString);
        db.BuildTransactCollections();
        _ = services.AddSingleton(typeof(IDbSet), db);
        _ = services.AddSingleton(db._database!);
        return services;
    }
}