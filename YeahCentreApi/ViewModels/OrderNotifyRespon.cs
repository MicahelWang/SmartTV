using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahCentreApi.ViewModels
{
    public class OrderNotifyRespon
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }

}