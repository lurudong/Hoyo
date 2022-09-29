using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hoyo.WebCore;
/// <summary>
/// JSON转换器
/// </summary>
public class SystemTextJsonConvert
{
    private const string DateFormat = "yyyy-MM-dd";
    private const string TimeFormat = "HH:mm:ss";
    private const string DateTimeFormat = $"{DateFormat} {TimeFormat}";

    /// <summary>
    /// Decimal数据类型Json转换(用于将字符串类型的数字转化成后端可识别的decimal类型)
    /// </summary>
    public class DecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType == JsonTokenType.Number ? reader.GetDecimal() : decimal.Parse(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(CultureInfo.CurrentCulture));
    }
    /// <summary>
    /// 可空Decimal数据类型Json转换(用于将字符串类型的数字转化成后端可识别的decimal类型)
    /// </summary>
    public class DecimalNullConverter : JsonConverter<decimal?>
    {
        public override decimal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetDecimal()
                : string.IsNullOrEmpty(reader.GetString())
                ? default(decimal?)
                : decimal.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString());
    }
    /// <summary>
    /// Int数据类型Json转换(用于将字符串类型的数字转化成后端可识别的int类型)
    /// </summary>
    public class IntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetInt32()
                : string.IsNullOrEmpty(reader.GetString()) ? default : int.Parse(reader.GetString()!);
        }
        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) => writer.WriteNumberValue(value);
    }
    /// <summary>
    /// 可空Int数据类型Json转换(用于将字符串类型的数字转化成后端可识别的int类型)
    /// </summary>
    public class IntNullConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Number
                ? reader.GetInt32()
                : string.IsNullOrEmpty(reader.GetString()) ? default(int?) : int.Parse(reader.GetString()!);
        }
        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value is not null) writer.WriteNumberValue(value.Value);
            else writer.WriteNullValue();
        }
    }
    /// <summary>
    /// Bool类型Json转换(用于将字符串类型的true或false转化成后端可识别的bool类型)
    /// </summary>
    public class BoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.True or JsonTokenType.False
                ? reader.GetBoolean()
                : reader.TokenType == JsonTokenType.String
                ? bool.Parse(reader.GetString()!)
                : reader.TokenType == JsonTokenType.Number
                ? reader.GetDouble() > 0
                : throw new NotImplementedException($"un processed token type {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
    }
    /// <summary>
    /// 可空Bool类型Json转换(用于将字符串类型的true或false转化成后端可识别的bool类型)
    /// </summary>
    public class BoolNullConverter : JsonConverter<bool?>
    {
        public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType is JsonTokenType.True or JsonTokenType.False
                ? reader.GetBoolean()
                : reader.TokenType == JsonTokenType.Null
                ? null
                : reader.TokenType == JsonTokenType.String
                ? bool.Parse(reader.GetString()!)
                : reader.TokenType == JsonTokenType.Number
                ? reader.GetDouble() > 0
                : throw new NotImplementedException($"un processed token type {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
        {
            if (value is not null) writer.WriteBooleanValue(value.Value);
            else writer.WriteNullValue();
        }
    }
    /// <summary>
    /// DateTime类型Json转换(用于将字符串类型的DateTime转化成后端可识别的时间类型)
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Convert.ToDateTime(reader.GetString());

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(DateTimeFormat));
    }
    /// <summary>
    /// 可空DateTime类型Json转换(用于将字符串类型的DateTime转化成后端可识别的时间类型)
    /// </summary>
    public class DateTimeNullConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrEmpty(reader.GetString()) ? null : Convert.ToDateTime(reader.GetString());

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(DateTimeFormat));
    }

#if !NETSTANDARD
    /// <summary>
    /// TimeOnly类型Json转换(用于将字符串类型的时间转化成后端可识别的TimeOnly类型)
    /// </summary>
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => TimeOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(TimeFormat));
    }
    /// <summary>
    /// 可空TimeOnly类型Json转换(用于将字符串类型的时间转化成后端可识别的TimeOnly类型)
    /// </summary>
    public class TimeOnlyNullJsonConverter : JsonConverter<TimeOnly?>
    {
        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrWhiteSpace(reader.GetString()) ? null : TimeOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(TimeFormat));
    }
    /// <summary>
    /// DateOnly类型Json转换(用于将字符串类型的日期转化成后端可识别的DateOnly类型)
    /// </summary>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(DateFormat));
    }
    /// <summary>
    /// 可空DateOnly类型Json转换(用于将字符串类型的日期转化成后端可识别的DateOnly类型)
    /// </summary>
    public class DateOnlyNullJsonConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => string.IsNullOrWhiteSpace(reader.GetString()) ? null : DateOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options) => writer.WriteStringValue(value?.ToString(DateFormat));
    }
#endif
}