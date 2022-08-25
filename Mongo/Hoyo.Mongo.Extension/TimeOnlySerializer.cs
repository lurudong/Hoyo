﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Hoyo.Mongo;
/// <summary>
/// TimeOnly序列化方式
/// </summary>
internal class TimeOnlySerializer : StructSerializerBase<TimeOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        var str = value.ToString("HH:mm:ss");
        context.Writer.WriteString(str);
    }

    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var ticks = context.Reader.ReadString();
        var success = TimeOnly.TryParse(ticks, out var result);
        return success ? result : throw new("不受支持的数据格式.");
    }
}