using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahCentreApi.ViewModels
{
    public class PostHotelInfoData
    {
        [JsonProperty("deviceid")]
        public string DeviceId { get; set; }
    }
}