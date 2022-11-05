namespace Hoyo.Extension.Validations
{
    ///  可以参考微软Microsoft.AspNetCore.Http.Extensions.ProblemDetails
    /// <summary>
    /// 标准类
    /// </summary>
    public class ProblemDetails
    {

        /// <summary>
        /// 类型
        /// </summary>

        public string Type { get; set; } = default!;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 错误
        /// </summary>

        public Dictionary<string, string[]> Errors { get; set; }
    }
}
