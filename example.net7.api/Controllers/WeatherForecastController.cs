using Hoyo.WebCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace example.net7.api.Controllers;

/// <summary>
/// ����mongodb��һЩ����
/// </summary>
[ApiController, Route("api/[controller]")]
[ ApiGroup("WeatherForecast","2022-09-28", "WeatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly DbContext db;
    private readonly FilterDefinitionBuilder<MongoTest> bf = Builders<MongoTest>.Filter;
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="dbContext"></param>
    public WeatherForecastController(DbContext dbContext)
    {
        db = dbContext;
    }

    /// <summary>
    /// ��MongoDB�в���.Net6+��������,�����Զ�ת���Ƿ���Ч
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
    /// ���Դ�MongoDB��ȡ�����������,�ٷ��ص�Swagger�鿴����JSONת���Ƿ�����
    /// </summary>
    /// <returns></returns>
    [HttpGet("MongoGet")]
    public async Task<IEnumerable<MongoTest>> MongoGet() => await db.Test.Find(bf.Empty).ToListAsync();
}