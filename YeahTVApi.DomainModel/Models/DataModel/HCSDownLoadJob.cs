using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSDownLoadJob : BaseEntity<string>
    {
        public string TaskId { get; set; }

        public string Name { get; set; }

        public string Operation { get; set; }

        public string MD5 { get; set; }

        public string Priority { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string Status { get; set; }

        public string BizNo { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string LastUpdateUser { get; set; }

        [JsonIgnore]
        public virtual HCSDownloadTask HCSDownloadTask { get; set; }

    }
}
