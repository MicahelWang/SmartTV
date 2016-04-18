using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class CommodityDetialList
    {
        [JsonProperty("pageTotal")]
        public int TotalPage { get; set; }
        [JsonProperty("pageindex")]
        public int PageIndex { get; set; }
        [JsonProperty("pagesize")]
        public int PageSize { get; set; }
        [JsonProperty("products")]
        public List<ProductLists> Products { get; set; }
    }
    public class ProductLists
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("number")]
        public string number { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("price")]
        public string price { get; set; }
        [JsonProperty("on_sale")]
        public string on_sale { get; set; }
        [JsonProperty("brand")]
        public string brand { get; set; }
        [JsonProperty("unit")]
        public string unit { get; set; }
        [JsonProperty("specification")]
        public string specification { get; set; }
        [JsonProperty("quantity")]
        public int quantity { get; set; }
        [JsonProperty("image_url")]
        public string image_url { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("stock_taking_time")]
        //[JsonConverter(typeof(TimestampConverterToString))]
        public string stock_taking_time { get; set; }

    }
}
