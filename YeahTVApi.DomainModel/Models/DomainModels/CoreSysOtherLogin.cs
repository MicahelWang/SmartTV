using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysOtherLogin : BaseEntity<string>
    {
        public string UserId { get; set; }
        public string OtherId { get; set; }
        public int Type { get; set; }
    }
}
