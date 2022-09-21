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
        var connstr = configuration["CONNECTIONSTRINGS_MONGO"];
        if (string.IsNullOrWhiteSpace(connstr)) connstr = configuration.GetConnectionString("Mongo");
        return string.IsNullOrWhiteSpace(connstr)
            ? throw new("💔:无 [CONNECTIONSTRINGS_MONGO] 系统环境变量或appsettings.json中不存在ConnectionStrings::Mongo配置")
            : connstr;
    }

    /// <summary>
    /// 通过默认连接字符串名称添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="options">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, IConfiguration configuration, Action<HoyoMongoOptions>? options = null) where T : BaseDbContext
    {
        var connstr = ConnectionString(configuration);
        _ = services.AddMongoDbContext<T>(connstr, options);
        return services;
    }

    /// <summary>
    /// 通过连接字符串添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="connstr">链接字符串</param>
    /// <param name="options">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, string connstr, Action<HoyoMongoOptions>? options = null) where T : BaseDbContext
    {
        var dboptions = new HoyoMongoOptions();
        options?.Invoke(dboptions);
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(connstr);
        _ = services.AddSingleton(db).AddSingleton(db.Database).AddSingleton(db.Client);
        return services;
    }

    /// <summary>
    /// 使用HoyoMongoClientSettings配置添加DbContext
    /// </summary>
    /// <typeparam name="T">Hoyo.Mongo.DbContext</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="settings">HoyoMongoClientSettings</param>
    /// <param name="options">DbContextOptions</param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbContext<T>(this IServiceCollection services, Action<HoyoMongoSettings> settings, Action<HoyoMongoOptions>? options = null) where T : BaseDbContext
    {
        var dboptions = new HoyoMongoOptions();
        var setting = new HoyoMongoSettings();
        options?.Invoke(dboptions);
        settings.Invoke(setting);
        BaseDbContext.RegistryConventionPack(dboptions);
        var db = BaseDbContext.CreateInstance<T>(setting);
        _ = services.AddSingleton(db).AddSingleton(db.Database).AddSingleton(db.Client);
        return services;
    }
}