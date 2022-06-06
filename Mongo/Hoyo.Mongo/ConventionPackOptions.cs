namespace Hoyo.Mongo;
public class ConventionPackOptions
{
    protected readonly List<Type> ConvertObjectIdToStringTypes = new();
    /// <summary>
    /// 添加ObjectId到String转换的类型[使用该方法添加的对象,不会将Id,ID字段转化为ObjectId类型.在数据库中存为字符串格式]
    /// </summary>
    /// <param name="types"></param>
    public void AddConvertObjectIdToStringTypes(params Type[] types) => ConvertObjectIdToStringTypes.AddRange(types);
    /// <summary>
    /// 判断一个对象是否是不允许将string类型的id自动转化为ObjectId类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsConvertObjectIdToStringType(Type type) => ConvertObjectIdToStringTypes.Contains(type);
}