using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSDownloadTask : BaseEntity<string>
    {
        //public string Id { get; set; }

        public string TaskNo { get; set; }

        public string ServerId { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string ResultStatus { get; set; }

        public string Config { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string LastUpdateUser { get; set; }

        public virtual ICollection<HCSDownLoadJob> HCSDownLoadJobs { get; set; }
    }
}
