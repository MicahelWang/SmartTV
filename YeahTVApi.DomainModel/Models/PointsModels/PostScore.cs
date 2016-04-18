using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.PointsModels
{
    public class PostScore
    {
         [JsonProperty("memberid")]
        public string Memberid { get; set; }

         [JsonProperty("orderid")]
        public string Orderid { get; set; }

         [JsonProperty("token")]
        public string Token { get; set; }

    }
}
