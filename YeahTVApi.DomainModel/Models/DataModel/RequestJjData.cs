using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class RequestJjData : RequestJjDataBase
    {
        [JsonProperty("channelsource")]
        public string ChannelSource { get; set; }
        [JsonProperty("productid")]
        public string ProductId { get; set; }
        [JsonProperty("productname")]
        public string ProductName { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("score")]
        public string Score { get; set; }
        [JsonProperty("orderid")]
        public string OrderId { get; set; }
        [JsonProperty("ext1")]
        public string Ext1 { get; set; }
        [JsonProperty("ext2")]
        public string Ext2 { get; set; }

    }
}
