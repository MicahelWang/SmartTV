using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Entity;

namespace YeahTVApi.DomainModel.Models.YeahHcsApi
{
    public class PostAppsListParameters : RequestHeader
    {
        [JsonProperty("appListRequestModelsString")]
        public List<AppListRequestModel> AppListRequestModels { get; set; }
    }
}