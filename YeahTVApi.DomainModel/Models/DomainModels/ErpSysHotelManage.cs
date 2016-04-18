using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public partial class ErpSysHotelManage : BaseEntity<string>
    {
        public string HotelId { get; set; }
        public string UserId { get; set; }
    }
}
