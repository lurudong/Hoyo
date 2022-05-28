#### Hoyo.Framework.NativeAssets

包含QRCode工具,由于绘图等一些操作需要平台依赖包支持,所以会较大,因此单独打包

#### 使用QRCode功能
- 使用 Nuget GUI工具添加至项目
- Install-Package Hoyo.Framework.NativeAssets

#### QRCode生成以及解析
```csharp
QRCode.GetBase64("hoyo.framework", width: 320, height: 320);
QRCode.QRDecoder(base64);
```