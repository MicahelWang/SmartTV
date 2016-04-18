using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 对象结果接口
    /// </summary>
    public class ApiObjectResult<T> : ApiResult
    {
        /// <summary>
        /// 返回对象
        /// </summary>
        public T obj { get; set; }

        /// <summary>
        /// 对象结果为错误
        /// </summary>
        /// <param name="err">错误类型</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiObjectResult<T> WithError(ApiErrorType err)
        {
            this.ResultType = ApiError.GetErrorCode(err);
            this.Message = ApiError.GetErrorDesc(err);
            return this;
        }

        /// <summary>
        /// 对象结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiObjectResult<T> WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 对象结果为正确
        /// </summary>
        /// <param name="val">对象的值</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与对象的值</returns>
        public ApiObjectResult<T> WithOk(T val, int code = 0)
        {
            this.ResultType = code;
            this.obj = val;
            base.Data = val;
            return this;
        }
    }
}
