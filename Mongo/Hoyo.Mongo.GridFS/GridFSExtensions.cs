using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Hoyo.Mongo.GridFS;
public static class GridFSExtensions
{
    internal static string BusinessApp { get; set; } = string.Empty;
    /// <summary>
    /// 注册GridFS服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="db">IMongoDatabase,为空情况下使用默认数据库hoyofs</param>
    /// <param name="fsOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddHoyoGridFS(this IServiceCollection services, IMongoDatabase? db = null, Action<HoyoGridFSOptions>? fsOptions = null)
    {
        var client = services.BuildServiceProvider().GetService<IMongoClient>();
        var options = new HoyoGridFSOptions();
        fsOptions?.Invoke(options);
        if (db is null)
        {
            options.DefaultDB = true;
            if (client is null) throw new("无法从容器中获取IMongoClient的服务依赖,请考虑是否使用Hoyo.Mongo包添加Mongodb服务或显示传入db参数.");
        }
        BusinessApp = options.BusinessApp;
        var hoyodb = options.DefaultDB ? client!.GetDatabase("hoyofs") : db;
        _ = services.Configure<FormOptions>(c =>
        {
            c.MultipartBodyLengthLimit = long.MaxValue;
            c.ValueLengthLimit = int.MaxValue;
        }).Configure<KestrelServerOptions>(c => c.Limits.MaxRequestBodySize = int.MaxValue).AddSingleton(new GridFSBucket(hoyodb, options.Options));
        _ = services.AddSingleton(hoyodb!.GetCollection<GridFSItemInfo>(options.ItemInfo));
        return services;
    }
}