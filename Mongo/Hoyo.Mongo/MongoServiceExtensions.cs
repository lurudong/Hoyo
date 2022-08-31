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
    private static string ConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration["CONNECTIONSTRINGS_MONGO"];
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
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, IConfiguration configuration, Action<HoyoMongoOptions>? dboptions = null) where T : BaseDbContext
    {
        var options = new HoyoMongoOptions();
        dboptions?.Invoke(options);
        var connectionString = ConnectionString(configuration);
        BaseDbContext.RegistryConventionPack(options);
        var db = BaseDbContext.CreateInstance<T>(connectionString);
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database);
        _ = services.AddSingleton(db._client);
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
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, Action<HoyoMongoSettings> settings, Action<HoyoMongoOptions>? dboptions = null) where T : BaseDbContext
    {
        var options = new HoyoMongoOptions();
        var setting = new HoyoMongoSettings();
        dboptions?.Invoke(options);
        settings.Invoke(setting);
        BaseDbContext.RegistryConventionPack(options);
        var db = BaseDbContext.CreateInstance<T>(setting);
        _ = services.AddSingleton(db);
        _ = services.AddSingleton(db._database);
        _ = services.AddSingleton(db._client);
        return services;
    }
}