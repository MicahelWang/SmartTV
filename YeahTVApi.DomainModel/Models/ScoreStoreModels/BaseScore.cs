using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ScoreStoreModels
{
   public class BaseScore
    {
        [JsonProperty("memberid")]
       public string UserId { get; set; }
    }
}
