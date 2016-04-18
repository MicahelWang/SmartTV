using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 结果集接口
    /// </summary>
    public class ApiListResult<T> : ApiResult
    {
        /// <summary>
        /// 返回结果集
        /// </summary>
        public List<T> list;

        /// <summary>
        /// 页面数量
        /// </summary>
        public int pageCount { get; set; }

        /// <summary>
        /// 页面记录数
        /// </summary>
        public int pageRecordCount { get; set; }

        /// <summary>
        /// 带有错误的结果集
        /// </summary>
        /// <param name="err">错误类型</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiListResult<T> WithError(ApiErrorType err)
        {
            this.ResultType = ApiError.GetErrorCode(err);
            this.Message = ApiError.GetErrorDesc(err);
            return this;
        }

        /// <summary>
        /// 带有错误的结果集
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">结果码</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new ApiListResult<T> WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 正确情况的结果集
        /// </summary>
        /// <param name="lst">结果集列表</param>
        /// <param name="code">结果码</param>
        /// <returns>返回列表与结果代码</returns>
        public ApiListResult<T> WithOk(List<T> lst, int code = 0)
        {
            this.ResultType = code;
            this.list = lst;
            base.Data = lst;
            return this;
        }

    }
}
