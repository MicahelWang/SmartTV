using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class RequestJjDataBase
    {
        [JsonProperty("mccode")]
        public string McCode { get; set; }
        [JsonProperty("reqtime")]
        public string Reqtime { get; set; }
        [JsonProperty("partnerid")]
        public string PartnerId { get; set; }
        [JsonProperty("sign")]
        public string Sign { get; set; }

    }
}
