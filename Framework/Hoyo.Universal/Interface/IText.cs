namespace Hoyo.Universal;

public interface IText
{
    string Text { get; set; }
    /// <summary>
    /// 生成Text字段值,用于关键词查找
    /// </summary>
    void BuildText();
}