using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class PostErrorNotifyData
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("exception")]
        public string Exception { get; set; }
    }
}