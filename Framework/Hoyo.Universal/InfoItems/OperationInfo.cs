namespace Hoyo.Universal;

/// <summary>
/// 操作信息
/// </summary>
public class OperationInfo
{
    public OperationInfo() { }
    public OperationInfo(bool done, DateTime? time) { Done = done; Time = time; }
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime? Time { get; set; } = DateTime.Now;
    /// <summary>
    /// 是否完成
    /// </summary>
    public bool Done { get; set; }
}