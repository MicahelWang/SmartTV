using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSTaskConfig
    {
        [JsonProperty("highspeed")]
        public string HighSpeed { get; set; }

        [JsonProperty("highspeed_start")]
        public string HighSpeedStart { get; set; }

        [JsonProperty("highspeed_end")]
        public string HighSpeedEnd { get; set; }

        [JsonProperty("lowspeed")]
        public string LowSpeed { get; set; }

        [JsonProperty("lowspeed_start")]
        public string LowSpeedStart { get; set; }

        [JsonProperty("lowspeed_end")]
        public string LowSpeedEnd { get; set; }
    }
}
