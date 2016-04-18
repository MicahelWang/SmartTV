using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class WxTicket
    {
        [JsonProperty("ticket")]
        public string TicketStr { get; set; }

        [JsonProperty("expire_seconds")]
        public int ExpireSeconds { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class WxErrorInfo
    {
        [JsonProperty("errcode")]
        public string Errcode { get; set; }

        [JsonProperty("errmsg")]
        public string Errmsg { get; set; }
    }
}
