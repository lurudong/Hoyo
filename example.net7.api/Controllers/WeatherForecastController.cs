using Hoyo.WebCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace example.net7.api.Controllers;

/// <summary>
/// 测试mongodb的一些功能
/// </summary>
[ApiController, Route("api/[controller]")]
[ ApiGroup("WeatherForecast","2022-09-28", "WeatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly DbContext db;
    private readonly FilterDefinitionBuilder<MongoTest> bf = Builders<MongoTest>.Filter;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext"></param>
    public WeatherForecastController(DbContext dbContext)
    {
        db = dbContext;
    }

    /// <summary>
    /// 向MongoDB中插入.Net6+新增类型,测试自动转换是否生效
    /// </summary>
    /// <returns></returns>
    [HttpPost("MongoPost")]
    public Task MongoPost()
    {
        var o = new MongoTest
        {
            DateTime = DateTime.Now,
            TimeSpan = TimeSpan.FromMilliseconds(50000d),
            DateOnly = DateOnly.FromDateTime(DateTime.Now),
            TimeOnly = TimeOnly.FromDateTime(DateTime.Now)
        };
        _ = db.Test.InsertOneAsync(o);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 测试从MongoDB中取出插入的数据,再返回到Swagger查看数据JSON转换是否正常
    /// </summary>
    /// <returns></returns>
    [HttpGet("MongoGet")]
    public async Task<IEnumerable<MongoTest>> MongoGet() => await db.Test.Find(bf.Empty).ToListAsync();
}