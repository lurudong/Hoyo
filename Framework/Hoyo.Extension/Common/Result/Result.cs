namespace Hoyo.Extension.Common.Result
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public T Value { get; set; }

        public int Code { get; set; } = (int)ResultCode.Success;

        public string Message { get; set; }





        /// <summary>
        /// 是否成功
        /// </summary>

        public bool IsSuccess => Code == (int)ResultCode.Success;

    }

    /// <summary>
    /// 返回码 自己定义
    /// </summary>
    public enum ResultCode : int
    {
        /// <summary>
        /// 成功
        /// </summary>

        Success = 2000,

        /// <summary>
        /// 失败
        /// </summary>
        Faulted = 5000

    }
}
