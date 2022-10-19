using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;

/// <summary>
/// 默认控制器,不带分组信息.
/// </summary>
[Route("api/[controller]"), ApiController]
public class DefaultController : ControllerBase
{
    /// <summary>
    /// 测试返回.net6+新类型
    /// </summary>
    /// <returns></returns>
    [HttpGet("NewType")]
    public object GetNewType() => new
    {
        Time = new TimeOnly(11, 30, 48),
        Date = new DateOnly(2021, 11, 11)
    };

    /// <summary>
    /// 测试新类型的转换
    /// </summary>
    /// <param name="new"></param>
    /// <returns></returns>
    [HttpPost("NewType")]
    public object PostNewType(NewType @new) => new
    {
        Date = DateOnly.Parse(@new.Date!).AddDays(1),
        Time = TimeOnly.Parse(@new.Time!).AddHours(-1),
        DateTime = DateTime.Parse(@new.DateTime).AddYears(1)
    };

    /// <summary>
    /// 测试抛出一个异常
    /// </summary>
    [HttpGet("Error")]
    public void Error() => throw new("Get an error");

    /// <summary>
    /// 测试返回空值
    /// </summary>
    /// <returns></returns>
    [HttpGet("Null")]
    public object? Null() => null;

    /// <summary>
    /// 测试默认值
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("DefaultValueTest")]
    public object Post(DefaultValueTest dto) => dto;
}
/// <summary>
/// 
/// </summary>
public class DefaultValueTest {
    /// <summary>
    /// 测试
    /// </summary>
    [DefaultValue("测试")]
    public string? Test { get; set; }
    /// <summary>
    /// 测试默认值
    /// </summary>
    
    public string? Name { get; set; } = "张三";
}

/// <summary>
/// 新类型
/// </summary>
public class NewType
{
    /// <summary>
    /// 时间,如: 22:23:52
    /// </summary>
    public string? Time { get; set; }
    /// <summary>
    /// 日期,如: 2022-09-29
    /// </summary>
    public string? Date { get; set; }
    /// <summary>
    /// 完整时间,如: "1994-05-08"
    /// </summary>
    public string DateTime { get; set; } = "1994-05-08";
}
