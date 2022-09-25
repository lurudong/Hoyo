﻿using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Hoyo.Framework.NativeAssets;

public static class QrCode
{
    /// <summary>
    /// 生成二维码(默认大小:320*320)
    /// </summary>
    /// <param name="text">文本内容</param>
    /// <param name="keepWhiteBorderPixelVal">白边处理(负值表示不做处理，最大值不超过真实二维码区域的1/10)</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static string GetBase64(string text, int keepWhiteBorderPixelVal = -1, int width = 320, int height = 320)
    {
        var bytes = QrCoder(text, null, keepWhiteBorderPixelVal, width, height);
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
    }
    /// <summary>
    /// 生成二维码(320*320)
    /// </summary>
    /// <param name="text">文本内容</param>
    /// <param name="logoImgae">Logo图片(缩放到真实二维码区域尺寸的1/6)</param>
    /// <param name="keepWhiteBorderPixelVal">白边处理(负值表示不做处理，最大值不超过真实二维码区域的1/10)</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <returns></returns>
    private static byte[] QrCoder(string text, byte[]? logoImgae = null, int keepWhiteBorderPixelVal = -1, int width = 320, int height = 320)
    {
        var qRCodeWriter = new QRCodeWriter();
        var hints = new Dictionary<EncodeHintType, object>
        {
            { EncodeHintType.CHARACTER_SET, "utf-8" },
            { EncodeHintType.QR_VERSION, 9 },
            { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L }
        };
        var bitMatrix = qRCodeWriter.encode(text, BarcodeFormat.QR_CODE, width, height, hints);
        var w = bitMatrix.Width;
        var h = bitMatrix.Height;
        var sKBitmap = new SKBitmap(w, h);
        var blackStartPointX = 0;
        var blackStartPointY = 0;
        var blackEndPointX = w;
        var blackEndPointY = h;
        #region --绘制二维码(同时获取真实的二维码区域起绘点和结束点的坐标)--
        using var sKCanvas = new SKCanvas(sKBitmap);
        var sKColorBlack = SKColor.Parse("000000");
        var sKColorWihte = SKColor.Parse("FFFFFF");
        sKCanvas.Clear(sKColorWihte);
        var blackStartPointIsNotWriteDown = true;
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                var flag = bitMatrix[x, y];
                if (!flag) continue;
                if (blackStartPointIsNotWriteDown)
                {
                    blackStartPointX = x;
                    blackStartPointY = y;
                    blackStartPointIsNotWriteDown = false;
                }
                blackEndPointX = x;
                blackEndPointY = y;
                sKCanvas.DrawPoint(x, y, sKColorBlack);
            }
        }
        #endregion
        var qrcodeRealWidth = blackEndPointX - blackStartPointX;
        var qrcodeRealHeight = blackEndPointY - blackStartPointY;
        #region -- 处理白边 --
        if (keepWhiteBorderPixelVal > -1)//指定了边框宽度
        {
            var borderMaxWidth = (int)Math.Floor((double)qrcodeRealWidth / 10);
            if (keepWhiteBorderPixelVal > borderMaxWidth)
            {
                keepWhiteBorderPixelVal = borderMaxWidth;
            }
            var nQrcodeRealWidth = width - keepWhiteBorderPixelVal - keepWhiteBorderPixelVal;
            var nQrcodeRealHeight = height - keepWhiteBorderPixelVal - keepWhiteBorderPixelVal;
            using var sKBitmap2 = new SKBitmap(width, height);
            using var sKCanvas2 = new SKCanvas(sKBitmap2);
            sKCanvas2.Clear(sKColorWihte);
            //二维码绘制到临时画布上时无需抗锯齿等处理(避免文件增大)
            sKCanvas2.DrawBitmap(sKBitmap, new SKRect
            {
                Location = new() { X = blackStartPointX, Y = blackStartPointY },
                Size = new() { Height = qrcodeRealHeight, Width = qrcodeRealWidth }
            },
            new SKRect
            {
                Location = new() { X = keepWhiteBorderPixelVal, Y = keepWhiteBorderPixelVal },
                Size = new() { Width = nQrcodeRealWidth, Height = nQrcodeRealHeight }
            });
            blackStartPointX = keepWhiteBorderPixelVal;
            blackStartPointY = keepWhiteBorderPixelVal;
            qrcodeRealWidth = nQrcodeRealWidth;
            qrcodeRealHeight = nQrcodeRealHeight;
            sKBitmap.Dispose();
            sKBitmap = sKBitmap2;
        }
        #endregion
        #region -- 绘制LOGO --
        if (logoImgae is not null && logoImgae.Length > 0)
        {
            using var sKBitmapLogo = SKBitmap.Decode(logoImgae);
            if (!sKBitmapLogo.IsEmpty)
            {
                var logoTargetMaxWidth = (int)Math.Floor((double)qrcodeRealWidth / 6);
                var logoTargetMaxHeight = (int)Math.Floor((double)qrcodeRealHeight / 6);
                var qrcodeCenterX = (int)Math.Floor((double)qrcodeRealWidth / 2);
                var qrcodeCenterY = (int)Math.Floor((double)qrcodeRealHeight / 2);
                var logoResultWidth = sKBitmapLogo.Width;
                var logoResultHeight = sKBitmapLogo.Height;
                if (logoResultWidth > logoTargetMaxWidth)
                {
                    var r = (double)logoTargetMaxWidth / logoResultWidth;
                    logoResultWidth = logoTargetMaxWidth;
                    logoResultHeight = (int)Math.Floor(logoResultHeight * r);
                }
                if (logoResultHeight > logoTargetMaxHeight)
                {
                    var r = (double)logoTargetMaxHeight / logoResultHeight;
                    logoResultHeight = logoTargetMaxHeight;
                    logoResultWidth = (int)Math.Floor(logoResultWidth * r);
                }
                var pointX = qrcodeCenterX - (int)Math.Floor((double)logoResultWidth / 2) + blackStartPointX;
                var pointY = qrcodeCenterY - (int)Math.Floor((double)logoResultHeight / 2) + blackStartPointY;
                using var sKCanvas3 = new SKCanvas(sKBitmap);
                using var sKPaint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.Medium,
                    IsAntialias = true
                };
                sKCanvas3.DrawBitmap(sKBitmapLogo, new SKRect
                {
                    Location = new() { X = 0, Y = 0 },
                    Size = new() { Height = sKBitmapLogo.Height, Width = sKBitmapLogo.Width }
                },
                new SKRect
                {
                    Location = new() { X = pointX, Y = pointY },
                    Size = new() { Height = logoResultHeight, Width = logoResultWidth }
                }, sKPaint);
            }
        }
        #endregion
        using var sKImage = SKImage.FromBitmap(sKBitmap);
        sKBitmap.Dispose();
        using var data = sKImage.Encode(SKEncodedImageFormat.Png, 75);
        var reval = data.ToArray();
        return reval;
    }
    /// <summary>
    /// 从Base64解析二维码
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public static string QrDecoder(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64)) throw new("base64 is null or empty");
        var data = base64[(base64.IndexOf(",", StringComparison.Ordinal) + 1)..];
        var ms = new MemoryStream(Convert.FromBase64String(data));
        using var sKManagedStream = new SKManagedStream(ms, true);
        using var sKBitmap = SKBitmap.Decode(sKManagedStream) ?? throw new("未识别的图片文件");
        var w = sKBitmap.Width;
        var h = sKBitmap.Height;
        var ps = w * h;
        var bytes = new byte[ps * 3];
        var byteIndex = 0;
        for (var x = 0; x < w; x++)
        {
            for (var y = 0; y < h; y++)
            {
                var color = sKBitmap.GetPixel(x, y);
                bytes[byteIndex + 0] = color.Red;
                bytes[byteIndex + 1] = color.Green;
                bytes[byteIndex + 2] = color.Blue;
                byteIndex += 3;
            }
        }
        var rGbLuminanceSource = new RGBLuminanceSource(bytes, w, h);
        var hybridBinarizer = new HybridBinarizer(rGbLuminanceSource);
        var binaryBitmap = new BinaryBitmap(hybridBinarizer);
        var hints = new Dictionary<DecodeHintType, object>
        {
            { DecodeHintType.CHARACTER_SET, "utf-8" }
        };
        var qRCodeReader = new QRCodeReader();
        var result = qRCodeReader.decode(binaryBitmap, hints);
        return result is not null ? result.Text : "";
    }
}
