﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Hoyo.Mongo.GridFS.Extension;
[ApiController]
[Route("[controller]")]
public class ExtensionController : GridFSController
{
    private readonly HoyoStaticFileSettings FileSetting;
    public ExtensionController(GridFSBucket bucket, IMongoCollection<GridFSItemInfo> collection, IConfiguration config) : base(bucket, collection)
    {
        FileSetting = config.GetSection(HoyoStaticFileSettings.Position).Get<HoyoStaticFileSettings>();
    }

    /// <summary>
    /// 获取虚拟目录的文件路径
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns></returns>
    [HttpGet("FileUri/{id}")]
    public virtual async Task<object> FileUri(string id)
    {
        if (string.IsNullOrWhiteSpace(FileSetting.PhysicalPath)) throw new("RealPath is null");
        var fi = await (await Bucket.FindAsync("{_id:ObjectId('" + id + "')}")).SingleOrDefaultAsync() ?? throw new("no data find");
        using var mongoStream = await Bucket.OpenDownloadStreamAsync(ObjectId.Parse(id), new() { Seekable = true });
        if (!Directory.Exists(FileSetting.PhysicalPath)) _ = Directory.CreateDirectory(FileSetting.PhysicalPath);
        using var fsWrite = new FileStream($"{FileSetting.PhysicalPath}{Path.DirectorySeparatorChar}{fi.Filename}", FileMode.Create);
        var buffer = new byte[1024 * 1024];
        while (true)
        {
            var readCount = await mongoStream.ReadAsync(buffer, 0, buffer.Length);
            fsWrite.Write(buffer, 0, readCount);
            if (readCount < buffer.Length) break;
        }
        return new
        {
            Uri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{FileSetting.VirtualPath}/{fi.Filename}"
        };
    }

    /// <summary>
    /// 清理缓存文件夹
    /// </summary>
    /// <returns></returns>
    [HttpDelete("ClearTempDir")]
    public virtual Task ClearDir()
    {
        if (!Directory.Exists(FileSetting.PhysicalPath)) return Task.CompletedTask;
        try
        {
            Directory.Delete(FileSetting.PhysicalPath, true);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 重命名文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="newName">新名称</param>
    /// <returns></returns>
    [HttpPut("{id}/Rename/{newname}")]
    public override Task Rename(string id, string newName)
    {
        var filename = Coll.Find(c => c.FileId == id).Project(c => c.FileName).SingleOrDefaultAsync().Result;
        var path = $"{FileSetting.PhysicalPath}{Path.DirectorySeparatorChar}{filename}";
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        _ = base.Rename(id, newName);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="ids">文件ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public override async Task<IEnumerable<string>> Delete(params string[] ids)
    {
        var files = (await base.Delete(ids)).ToList();
        Task DeleteSingleFile()
        {
            foreach (var path in files.Select(item => $"{FileSetting.PhysicalPath}{Path.DirectorySeparatorChar}{item}").Where(System.IO.File.Exists))
            {
                System.IO.File.Delete(path);
            }
            return Task.CompletedTask;
        }
        _ = files.Count > 6 ? Task.Run(DeleteSingleFile) : DeleteSingleFile();
        return files;
    }
}
