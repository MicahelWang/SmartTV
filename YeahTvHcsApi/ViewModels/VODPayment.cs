using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class VODPayment
    {

        [JsonProperty("deviceseries")]
        public string DeviceSeries { get; set; }
        [JsonProperty("movieid")]
        public string MovieId { get; set; }

        [JsonProperty("paytype")]
        public string PayType { get; set; }
    }
}