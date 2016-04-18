using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeahTvHcsApi.ViewModels
{
    public class PerformanceData
    {
        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("values")]
        public JArray Values { get; set; }
    }
}