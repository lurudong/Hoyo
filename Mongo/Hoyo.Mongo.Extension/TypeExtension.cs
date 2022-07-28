using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Hoyo.Mongo.Extension;
public static class TypeExtension
{
    /// <summary>
    /// 添加MongoDB扩展类型转化支持,如.Net6+新增TimeOnly和DateOnly
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddTypeExtension(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new DateOnlySerializer());
        BsonSerializer.RegisterSerializer(new TimeOnlySerializer());
        return services;
    }
}
