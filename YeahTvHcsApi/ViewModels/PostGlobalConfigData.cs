using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class PostGlobalConfigData
    {
        [JsonProperty("old_config_no")]
        public string OldConfigNo;
    }
}