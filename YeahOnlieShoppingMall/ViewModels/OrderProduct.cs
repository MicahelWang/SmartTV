using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace YeahOnlieShoppingMall.ViewModels
{
    /// <summary>
    /// 商品实体
    /// </summary>
    public class OrderProduct
    {
        public OrderProduct()
        {

        }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
    /// <summary>
    /// 支付结果
    /// </summary>
    public class OrderResult
    {
        public OrderResult()
        {

        }
         [JsonProperty("resultcode")]
        public int Resultcode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

}