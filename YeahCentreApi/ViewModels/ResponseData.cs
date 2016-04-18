using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YeahTVApi.Common;

namespace YeahCentreApi.ViewModels
{
    public class ResponseData<T>
    {
        [JsonProperty("sign")]
        public string Sign { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}