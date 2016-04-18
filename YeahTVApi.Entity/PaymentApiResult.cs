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
    public class PaymentApiResult : ApiResult
    {

        public string OrderId { get; set; }


        public PaymentApiResult()
            : base()
        {
        }

        /// <summary>
        /// 结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="orderId"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与结果信息</returns>
        public PaymentApiResult WithError(string message, string orderId, int code = -1)
        {
            this.OrderId = orderId;
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public PaymentApiResult WithOk(string orderId, int code = 0)
        {
            this.ResultType = code;
            this.OrderId = orderId;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="message"></param>
        /// <param name="orderId"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public PaymentApiResult WithOk(string message, string orderId, int code = 0)
        {
            this.OrderId = orderId;
            this.ResultType = code;
            this.Message = message;
            return this;
        }
    }
}
