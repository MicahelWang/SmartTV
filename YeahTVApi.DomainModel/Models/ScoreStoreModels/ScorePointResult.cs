using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApi.DomainModel.Models.ScoreStoreModels
{
    public class ScorePointResult : SpecialJjReturn
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
