using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Hoyo.Mongo.Extension;
/// <summary>
/// 服务注册扩展类
/// </summary>
public static class RegisterSerializerExtension
{
    /// <summary>
    /// 添加MongoDB类型转化支持,如.Net6+新增TimeOnly和DateOnly
    /// 默认将时间本地化
    /// </summary>
    /// <param name="_"></param>
    /// <returns></returns>
    public static void RegisterHoyoSerializer(this IServiceCollection _)
    {
        BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));//to local time
        BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));//decimal to decimal default
        BsonSerializer.RegisterSerializer(new DateOnlySerializer());
        BsonSerializer.RegisterSerializer(new TimeOnlySerializer());
    }
    /// <summary>
    /// 添加自定义序列化规则
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="services">IServiceCollection</param>
    /// <param name="serializer">自定义序列化类</param>
    /// <returns></returns>
    public static IServiceCollection RegisterHoyoSerializer<T>(this IServiceCollection services, IBsonSerializer<T> serializer)
    {
        BsonSerializer.RegisterSerializer(serializer);
        return services;
    }
}
