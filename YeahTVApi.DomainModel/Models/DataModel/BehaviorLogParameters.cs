using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Entity;

namespace YeahTVApi.DomainModel.Models
{
    public class BehaviorLogParameters : RequestHeader
    {
        [JsonProperty("behaviorLogRequestString")]
        public List<BehaviorLogRequestNew> BehaviorLogRequestString { get; set; }
    }
}