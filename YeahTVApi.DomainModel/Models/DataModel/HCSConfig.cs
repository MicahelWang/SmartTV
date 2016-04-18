using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSConfig : BaseEntity<string>
    {
        public string ServerId { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
    }
}
