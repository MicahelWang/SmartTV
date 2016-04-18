using System.Runtime.Serialization;
namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 操作结果父类
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        ///     获取或设置 操作结果类型
        /// </summary>
        public OperationResultType ResultType { get; set; }

        /// <summary>
        ///     获取或设置 操作返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     获取或设置 操作返回的详细信息，用于记录日志等
        /// </summary>
        public string MoreInfo { get; set; }

        /// <summary>
        /// 系统记录日志的编号
        /// </summary>
        public string ErrorKey { get; set; }
    }
}
