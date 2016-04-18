using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.PointsModels
{
    public class ResponseScore
    {
         [JsonProperty("score")]
        public int Score { get; set; }

         [JsonProperty("name")]
        public string Name { get; set; }
    }
}
