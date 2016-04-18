using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahTVIntegralExchange.Models
{
    public class OrderModels
    {
        [JsonProperty("orderid")]
        public string orderid { get; set; }
        [JsonProperty("score")]
        public int score { get; set; }
        [JsonProperty("memberid")]
        public string memberid { get; set; }
        [JsonProperty("sign")]
        public string sign { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("curscore")]
        public int CurScore { get; set; }
        [JsonProperty("expiredMinus")]
        public int ExpiredMinus { get; set; }
        [JsonProperty("ticket")]
        public string Ticket { get; set; }
    }
}