using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public partial class HCSTask
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("task_no")]
        public string TaskNo { get; set; } 

        [JsonIgnore]
        public string ServerId { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string Status { get; set; }

        [JsonIgnore]
        public string ResultStatus { get; set; }

        [JsonProperty("config")]
        public HCSTaskConfig Config { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public DateTime CreateTime { get; set; }

        [JsonIgnore]
        public DateTime UpdateTime { get; set; }

        [JsonIgnore]
        public string LastUpdateUser { get; set; }

        [JsonProperty("jobs")]
        public ICollection<HCSJob> Jobs { get; set; }
    }
}
