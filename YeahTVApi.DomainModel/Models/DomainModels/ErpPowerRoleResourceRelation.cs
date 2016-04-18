using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class ErpPowerRoleResourceRelation:BaseEntity<string>
    {
        public string RoleId { get; set; }
        public int ResourceId { get; set; }
    }
}
