using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class HCSCacheVersion : BaseEntity<string>
    {
        public HCSCacheVersion()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public int Version { get; set; }
        public string PermitionType { get; set; }
        public string TypeId { get; set; }
        public string LastUpdateUser { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
