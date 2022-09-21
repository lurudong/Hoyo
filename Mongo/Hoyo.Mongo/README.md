#### Hoyo.Mongo

- 一个 MongoDB 驱动的服务包,方便使用 MongoDB 数据库.
- 数据库中字段名驼峰命名,ID,Id 自动转化成 ObjectId.
- 可配置部分类的 Id 字段不存为 ObjectId,而存为 string 类型.
- 自动本地化 MOngoDB 时间类型
- 添加.Net6 Date/Time Only类型支持(TimeOnly理论上应该是兼容原TimeSpan数据类型).
- Date/Time Only类型可结合[Hoyo.WebCore](https://github.com/joesdu/Hoyo.WebCore)使用,前端可直接传字符串类型的Date/Time Only的值.
---

##### 如何使用?

- 在系统环境变量或者 Docker 容器中设置环境变量名称为: CONNECTIONSTRINGS_MONGO = mongodb 链接字符串 或者在 appsetting.json 中添加,
- 现在你也可以参考example.api项目查看直接传入相关数据.

```json
{
  "ConnectionStrings": {
    "Mongo": "mongodb链接字符串"
  },
  // 或者使用
  "CONNECTIONSTRINGS_MONGO": "mongodb链接字符串"
}
```

##### 使用 Hoyo.Mongo ?

- 使用 Nuget 安装 Hoyo.Mongo
- .Net 6 +

```csharp
using example.api;
using Hoyo.Mongo;
using Hoyo.WebCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加Mongodb数据库服务
var dboptions = new HoyoMongoOptions();
dboptions.AppendConventionRegistry("IdentityServer Mongo Conventions", new()
{
    Conventions = new()
    {
        new IgnoreExtraElementsConvention(true)
    },
    Filter = _ => true
});

builder.Services.AddMongoDbContext<DbContext>(clientSettings: new()
{
    ServerAddresses = new()
    {
        new("192.168.2.10", 27017),
    },
    AuthDatabase = "admin",
    DatabaseName = "miracle",
    UserName = "oneblogs",
    Password = "&oneblogs.cn",
}, dboptions: dboptions)
.AddTypeExtension();// 用于添加新增类型的支持.

...
var app = builder.Build();
```