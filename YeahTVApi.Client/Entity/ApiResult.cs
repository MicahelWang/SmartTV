namespace YeahTVApi.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Newtonsoft;
    using Newtonsoft.Json;

    /// <summary>
    /// 结果接口
    /// </summary>
    public class ApiResult : ActionResult, IApiResult
    {
        /// <summary>
        /// 返回值 >=0 成功 <0 失败
        /// </summary>
        public int ResultType { get; set; }

        /// <summary>
        /// 返回信息，当Code < 0 时候，代表错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置 操作返回的详细信息，用于记录日志等 
        /// </summary>
        public string MoreInfo { get; set; }

        [JsonIgnore]
        public object Data { get; set; }
        /// <summary>
        /// 数据的时间戳
        /// </summary>
        public String TimeStamp { get; set; }

        /// <summary>
        /// 表示执行结果成功还是失败
        /// </summary>
        /// <returns>True 代表包含一个成功结果 False 代表包含一个错误结果</returns>
        [JsonIgnore]
        public bool isOk
        {
            get
            {
                return (ResultType >= 0);
            }
        }

        /// <summary>
        /// 清空结果接口
        /// </summary>
        public ApiResult()
        {
            SetResult(0, string.Empty);
        }
        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="code">结果代码</param>
        /// <param name="message">结果信息</param>
        private void SetResult(int code, string message)
        {
            this.ResultType = code;
            this.Message = message;
        }
        /// <summary>
        /// 设置结果为错误
        /// </summary>
        /// <param name="message">结果信息</param>
        /// <param name="code">结果代码</param>
        public void SetErrorResult(string message, int code = -1)
        {
            SetResult(code, message);
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context">结果内容</param>
        public override void ExecuteResult(ControllerContext context)
        {
            //context.ToString();
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context">结果内容</param>
        public ApiResult WithResult(IFunResult rst)
        {
            this.ResultType = rst.ResultType;
            this.Message = rst.Message;
            return this;
        }
        /// <summary>
        /// 结果为错误
        /// </summary>
        /// <param name="err">错误类型</param>
        /// <returns>返回结果代码与结果信息</returns>
        public ApiResult WithError(ApiErrorType err)
        {
            this.ResultType = ApiError.GetErrorCode(err);
            this.Message = ApiError.GetErrorDesc(err);
            return this;
        }
        /// <summary>
        /// 结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与结果信息</returns>
        public ApiResult WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }
        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public ApiResult WithOk(int code = 0)
        {
            this.ResultType = code;
            return this;
        }
    }
}
