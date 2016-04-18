using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahCentreApi.ViewModels
{
    public class StoreOrderState
    {
        [JsonProperty("hotelid",Order=1)]
        public string HotelId { get; set; }

        [JsonProperty("orderid",Order=2)]
        public string OrderId { get; set; }

        [JsonProperty("status",Order=0)]
        public string Status { get; set; }
    }
}