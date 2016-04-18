using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class PostTestDataNotifyData
    {
        [JsonProperty("ServerId")]
        public string ServerId { get; set; }

        [JsonProperty("biz_type")]
        public string BizType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("biz_info")]
        public string BizInfo { get; set; }
    }
}