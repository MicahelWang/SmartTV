using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class HCSPostParameters<T>
    {
        [JsonProperty("server_id")]
        public string Server_Id { get; set; }

        [JsonProperty("sign")]
        public string Sign { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
