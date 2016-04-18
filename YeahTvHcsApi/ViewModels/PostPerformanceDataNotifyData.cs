using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class PostPerformanceDataNotifyData
    {
        [JsonProperty("ServerId")]
        public string ServerId { get; set; }

        [JsonProperty("CPU")]
        public PerformanceData CPU { get; set; }

        [JsonProperty("MEM")]
        public PerformanceData Memory { get; set; }

        [JsonProperty("DISK")]
        public PerformanceData Disk { get; set; }
    }
}