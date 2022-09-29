using Hoyo.Framework.NativeAssets;
using Hoyo.WebCore.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers;

/// <summary>
/// 测试二维码功能
/// </summary>
[Route("api/[controller]"), ApiController]
[ApiGroup(title: "QrCode", "2022-09-29", "二维码编码和解码")]
public class QrCodeController : ControllerBase
{
    /// <summary>
    /// 传入一个文本,并返回二维码的Base64字符串
    /// </summary>
    /// <param name="text">文本内容</param>
    /// <returns></returns>
    [HttpGet("QRCode")]
    public object GetQRCode(string text) => QrCode.GetBase64(text, width: 320, height: 320);

    /// <summary>
    /// 传入二维码Base64字符串,解析其中的信息
    /// </summary>
    /// <param name="base64">二维码Base64字符串.</param>
    /// <returns></returns>
    [HttpGet("QRDecoder")]
    public object QRDecoder(string base64) => QrCode.QrDecoder(base64);
}
