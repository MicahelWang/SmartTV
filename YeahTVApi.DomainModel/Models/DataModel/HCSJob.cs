using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSJob
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonIgnore]
        public string TaskId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("md5")]
        public string MD5 { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonIgnore]
        public string Status { get; set; }

        [JsonIgnore]
        public string BizNo { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public string CreateTime { get; set; }

        [JsonIgnore]
        public string UpdateTime { get; set; }

        [JsonIgnore]
        public string LastUpdateUser { get; set; }
    }
}
