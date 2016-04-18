using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 字符结果接口
    /// </summary>
    public class ApiStringResult : ApiResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 字符结果为错误
        /// </summary>
        /// <param name="err">错误类型</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiStringResult WithError(ApiErrorType err)
        {
            this.ResultType = ApiError.GetErrorCode(err);
            this.Message = ApiError.GetErrorDesc(err);
            return this;
        }

        /// <summary>
        /// 字符结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiStringResult WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 字符结果为正确
        /// </summary>
        /// <param name="val">结果值</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果值与结果代码</returns>
        public ApiStringResult WithOk(string val, int code = 0)
        {
            this.value = val;
            this.ResultType = code;
            base.Data = val;
            return this;
        }
    }
}
