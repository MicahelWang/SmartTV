using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class BackData
    {
        [JsonProperty("resultcode")]
        public int ResultCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

    }
}
