using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahCentreApi.ViewModels
{
    public class PermitionRequest
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }
    }
}