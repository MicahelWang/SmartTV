using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahOnlieShoppingMall.ViewModels
{
    
    public class OrderProducts
    {
        public OrderProducts()
        {
        }
        [JsonProperty("products")]
        public List<OrderProduct> Products { get; set; }
    }
}