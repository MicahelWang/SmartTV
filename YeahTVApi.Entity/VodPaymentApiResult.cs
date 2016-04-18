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
    public class VodPaymentApiResult : ApiResult
    {
        [JsonProperty("OrderPrice",Order=3)]
        public string OrderPrice { get; set; }


        public VodPaymentApiResult()
            : base()
        {
            this.OrderPrice = "";
        }

        /// <summary>
        /// 结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="orderPrice"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与结果信息</returns>
        public VodPaymentApiResult WithError(string message,  int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="orderPrice"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public VodPaymentApiResult WithOk(int code = 0)
        {
            this.ResultType = code;
            return this;
        }

        /// <summary>
        /// 结果为正确
        /// </summary>
        /// <param name="message"></param>
        /// <param name="orderPrice"></param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码</returns>
        public VodPaymentApiResult WithOk(string message, string orderPrice, int code = 0)
        {
            this.OrderPrice = orderPrice;
            this.ResultType = code;
            this.Message = message;
            return this;
        }
    }
}
