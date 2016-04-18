using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ScoreStoreModels
{
    public class RequestScore : BaseScore
    {
        [JsonProperty("orderid")]
        public string OrderId { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
