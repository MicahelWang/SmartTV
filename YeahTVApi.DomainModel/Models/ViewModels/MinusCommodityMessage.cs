using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class MinusCommodityMessage
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
