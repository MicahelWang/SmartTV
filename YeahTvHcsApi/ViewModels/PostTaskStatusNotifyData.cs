using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using YeahTvHcsApi.Common.HttpParameterBinding;

namespace YeahTvHcsApi.ViewModels
{
    //[TypeConverter(typeof(JsonDataConverter<PostTaskStatusNotifyParameters>))]
    public class PostTaskStatusNotifyData
    {
        [JsonProperty("biz_type")]
        public string BizType { get; set; }

        [JsonProperty("biz_no")]
        public string BizNo { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("err_msg")]
        public string ErrorMessage { get; set; }
    }
}