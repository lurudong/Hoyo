using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Hoyo.Mongo.GridFS;
public static class GridFSExtensions
{
    public static string BusinessApp { get; set; } = string.Empty;
    public static IServiceCollection AddHoyoGridFS(this IServiceCollection services, IMongoDatabase db, HoyoGridFSOptions? fsoptions = null)
    {
        if (db is null) throw new("db can't be null");
        fsoptions ??= new();
        BusinessApp = fsoptions.BusinessApp;
        var hoyodb = fsoptions.DefalutDB ? db.Client.GetDatabase("hoyo") : db;
        _ = services.Configure<FormOptions>(c =>
        {
            c.MultipartBodyLengthLimit = long.MaxValue;
            c.ValueLengthLimit = int.MaxValue;
        }).Configure<KestrelServerOptions>(c => c.Limits.MaxRequestBodySize = int.MaxValue).AddSingleton(new GridFSBucket(hoyodb, fsoptions.Options));
        _ = services.AddSingleton(hoyodb.GetCollection<GridFSItemInfo>(fsoptions.ItemInfo));
        return services;
    }
}