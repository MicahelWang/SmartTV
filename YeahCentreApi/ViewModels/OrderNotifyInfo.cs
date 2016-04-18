using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.DomainModel.Models;

namespace YeahCentreApi.ViewModels
{
    public class OrderNotifyInfo
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("roomNum")]
        public string RoomNum { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("orderTime")]
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime OrderTime { get; set; }

        [JsonProperty("payType")]
        public string PayType { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("statustext")]
        public string StatusText { get; set; }
        [JsonProperty("goods")]
        public List<GoodsInfo> Goods { get; set; }
        [JsonProperty("hotelid")]
        public string HotelId { get; set; }
        [JsonProperty("deliveryType")]
        public string DeliveryType { get; set; }
    }

    public class GoodsInfo
    {
        [JsonProperty("goodsId")]
        public string GoodsId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}