using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using YeahTvHcsApi.Common.HttpParameterBinding;
using YeahTVApi.Common;

namespace YeahTvHcsApi.ViewModels
{
    public class ResponseData<T>
    {
        [JsonProperty("sign")]
        public string Sign { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}