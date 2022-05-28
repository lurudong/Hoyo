using Hoyo.Framework.NativeAssets;
using Hoyo.Tools;
using Microsoft.AspNetCore.Mvc;

namespace example.api.Controllers;

[ApiController, Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet("QRCode")]
    public object GetQRCode(string text) => QRCode.GetBase64(text, width: 320, height: 320);

    [HttpGet("QRDecoder")]
    public object QRDecoder(string base64) => QRCode.QRDecoder(base64);
    [HttpGet("SnowFlake")]
    public object SnowFlake() => SnowflakeId.GenerateNewId();
}