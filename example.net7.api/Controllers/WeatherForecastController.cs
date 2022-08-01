using Hoyo.Framework.NativeAssets;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace example.net7.api.Controllers;

[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly DbContext db;
    private readonly FilterDefinitionBuilder<MongoTest> bf = Builders<MongoTest>.Filter;

    public WeatherForecastController(DbContext dbContext)
    {
        db = dbContext;
    }

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
    [HttpGet("MongoGet")]
    public async Task<IEnumerable<MongoTest>> MongoGet() => await db.Test.Find(bf.Empty).ToListAsync();

    [HttpGet("QRCode")]
    public object GetQRCode(string text) => QRCode.GetBase64(text, width: 320, height: 320);

    [HttpGet("QRDecoder")]
    public object QRDecoder(string base64) => QRCode.QRDecoder(base64);

    [HttpGet("NewType")]
    public object GetNewType() => new
    {
        Time = new TimeOnly(11, 30, 48),
        Date = new DateOnly(2021, 11, 11)
    };

    [HttpPost("NewType")]
    public object PostNewType(NewType @new) => new
    {
        Date = DateOnly.Parse(@new.Date!).AddDays(1),
        Time = TimeOnly.Parse(@new.Time!).AddHours(-1),
        DateTime = DateTime.Parse(@new.DateTime).AddYears(1)
    };

    [HttpGet("Error")]
    public void Error() => throw new("Get an error");

    [HttpGet("Null")]
    public object? Null() => null;
}

public class NewType
{
    public string? Time { get; set; }
    public string? Date { get; set; }
    public string DateTime { get; set; } = "1994-05-08";
}