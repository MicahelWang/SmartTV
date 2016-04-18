using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ScoreStoreModels
{
    public class RequestTokenParameter
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
