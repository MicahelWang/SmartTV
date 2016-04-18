using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class ErpSysRelationManage : BaseEntity<string>
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string ObjectId { get; set; }
    }
}
