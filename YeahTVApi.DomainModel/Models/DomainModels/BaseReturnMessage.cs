using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public class BaseReturnMessage
    {
        [JsonProperty("code")]
        public int Code { get; set; }

         [JsonProperty("message")]
        public string Message { get; set; }
    }
}
