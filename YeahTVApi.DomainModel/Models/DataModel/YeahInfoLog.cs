using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Entity;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class YeahInfoLog : RequestHeader
    {
        [JsonProperty("YeahInfoLog")]
        public List<TvLogInfo> YeahInfoLogs { get; set; }

    }
    public class TvLogInfo 
    {
        public string logFileName { get; set; }
        public string logType { get; set; }
        public string logContent { get; set; }

    }
}
