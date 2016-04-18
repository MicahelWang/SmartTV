using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class GoodsInfomation
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("brand")]
        public string Brand { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
        [JsonProperty("specification")]
        public string Specification { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("on_sale")]
        public string OnSale { get; set; }

        [JsonProperty("stock_taking_time")]
        //[JsonConverter(typeof(TimestampConverterToString))]
        public string  Stock_taking_time { get; set; }

    }
}
