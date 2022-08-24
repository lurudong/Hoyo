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
    private static string ConnectionString(IConfiguration configuration, string connKey)
    {
        var connectionString = configuration[connKey];
        if (string.IsNullOrWhiteSpace(connectionString)) connectionString = configuration.GetConnectionString("Mongo");
        return string.IsNullOrWhiteSpace(connectionString)
            ? throw new("💔:无 [CONNECTIONSTRINGS_MONGO] 系统环境变量或appsettings.json中不存在ConnectionStrings:Mongo配置")
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
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, IConfiguration configuration, HoyoMongoOptions? dboptions = null, string connKey = "CONNECTIONSTRINGS_MONGO") where T : BaseDbContext
    {
        dboptions ??= new();
        var connectionString = ConnectionString(configuration, connKey);
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(connectionString);
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database);
        return services;
    }

    /// <summary>
    /// 使用HoyoMongoClientSettings配置添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="settings">HoyoMongoClientSettings</param>
    /// <param name="dboptions">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, HoyoMongoSettings settings, HoyoMongoOptions? dboptions = null) where T : BaseDbContext
    {
        dboptions ??= new();
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(settings);
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database);
        return services;
    }
}